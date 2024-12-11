using Microsoft.EntityFrameworkCore;
using PragmaticSegmenterNet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using VkCore.Interfaces;
using VkCore.Models.Organization;
using VkCore.Models.TagModel;
using VkCore.Requests.ReadingRequest;
using VkCore.Services;
using VkCore.SharedKernel;

namespace VkCore.Models.ReadingModel
{
    public class Reading : BaseEntity, IResponseErrorLogger, IIsSoftDeleted
    {
        private List<string> _errors = new List<string>();
        private List<ContentItem> _contentItems = new List<ContentItem>();
        private HashSet<ReadingTag> _tags;
        [NotMapped]
        private List<ContentItem> _sortedContentItems
        {
            get
            {
                _contentItems.Sort((x, y) => x.BodyIndex.CompareTo(y.BodyIndex));
                return _contentItems;
            }
        }

        public string OrgId { get; set; }
        public string Title { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<ReadingTag> Tags => _tags?.ToList();
        public List<OrgReading> OrgReadings { get; } = new List<OrgReading>();

        [NotMapped]
        public IEnumerable<string> Errors => _errors;
        [NotMapped]
        public IEnumerable<ContentItem> ContentItems
        {
            get
            {
                var _local = _sortedContentItems;

                _local[0].SetFirstIndex(0);
                for (int i = 1; i < _local.Count; i++)
                {
                    _local[i].SetFirstIndex(_local[i - 1].LastIndex + 1);
                }

                return _local;
            }
        }

        private Reading() { }

        public Reading(string title, string text)
        {
            Id = Guid.NewGuid().ToString();
            this.Title = title;
            this.AppendContent(text ?? "");
            _tags = new HashSet<ReadingTag>();
        }

        public string Text
        {
            get
            {
                var text = new StringBuilder("");
                foreach (var item in _sortedContentItems)
                {
                    text.Append(item.Value);
                }
                return text.ToString();
            }
        }

        #region Tags

        public void AddTag(Tag tag, DbContext context = null)
        {
            if (_tags == null)
            {
                NullCheckContext(context, "tags");
                context.Entry(this).Collection(o => o.Tags).Load();
            }

            if (_tags.Any(x => x.TagId == tag.Id))
            {
                return;
            }

            _tags.Add(new ReadingTag(this.Id, tag));
        }

        public void RemoveTag(Tag tag, DbContext context = null)
        {
            if (_tags == null)
            {
                NullCheckContext(context, "tags");
                context.Entry(this).Collection(o => o.Tags).Load();
            }

            _tags.RemoveWhere(x => x.TagId == tag.Id);
        }

        #endregion

        #region Content

        public void AppendDefinition(string value, string wordId, DbContext context = null)
        {
            LoadBody(context);
            _contentItems.Add(new ContentItem(_sortedContentItems.Count, value, wordId));
        }

        public void AppendContent(string value, DbContext context = null)
        {
            LoadBody(context);
            ContentItem newContentItem;
            if (!_contentItems.Any())
            {
                // empty body, so just add one
                newContentItem = new ContentItem(0, value);
                _contentItems.Add(newContentItem);
                return;
            }

            if (_sortedContentItems[_contentItems.Count - 1].IsDefined())
            {
                // last item is a defined word
                // so push text into new CI
                newContentItem = new ContentItem(_contentItems.Count, value);
                _contentItems.Add(newContentItem);
                return;
            }

            // last item is just text, so append to value
            _sortedContentItems[_contentItems.Count - 1].Value += value;
        }

        public void InsertContent(int insertIndex, string value, DbContext context = null)
        {
            LoadBody(context);
            if (insertIndex == 0)
            {
                // we're at the beginning or end
                var newContentItem = new ContentItem(0, value);
                ShiftBodyIndexes(0);
                _contentItems.Add(newContentItem);
                return;
            }

            if (insertIndex >= Text.Length)
            {
                // we're at the beginning or end
                var newContentItem = new ContentItem(_contentItems.Count, value);
                _contentItems.Add(newContentItem);
                return;
            }

            var contentItemToSplit = ContentItems.FirstOrDefault(x => x.FirstIndex <= insertIndex && x.LastIndex >= insertIndex);
            if (contentItemToSplit == null)
                throw new ArgumentException($"Index {insertIndex} does not exist in this text.  Cannot insert here.");

            //var newContentItems = contentItemToSplit.SplitOutNewValue(insertIndex, value);
            //_contentItems.AddRange(newContentItems);
            //ShiftBodyIndexes(contentItemToSplit.BodyIndex + 3, 2);

            contentItemToSplit.InsertValue(insertIndex, value);
        }

        public void Delete(int start, int end, DbContext context = null)
        {
            LoadBody(context);
            // one single CI was deleted
            if (ContentItems.Any(x => x.FirstIndex == start && x.LastIndex == end))
            {
                var ciToDelete = ContentItems.First(x => x.FirstIndex == start && x.LastIndex == end);
                ShiftBodyIndexes(ciToDelete.BodyIndex, -1);
                _contentItems.Remove(ciToDelete);
                return;
            }

            // deletion happened inside a single CI
            if (ContentItems.Any(x => x.FirstIndex <= start && x.LastIndex >= end))
            {
                var ciToModify = ContentItems.First(x => x.FirstIndex <= start && x.LastIndex >= end);
                ciToModify.DeleteFromValue(start, end);
                return;
            }

            // deletion happened across multiple CIs
            if (ContentItems.Any(x => x.LastIndex >= start && x.FirstIndex <= end))
            {
                var cisToModify = ContentItems.Where(x => x.LastIndex >= start && x.FirstIndex <= end).ToList();

                var first = cisToModify.First(x => x.BodyIndex == cisToModify.Min(y => y.BodyIndex));
                first.DeleteFromValue(start, first.LastIndex);

                if (cisToModify.Count >= 2)
                {
                    var last = cisToModify.First(x => x.BodyIndex == cisToModify.Max(y => y.BodyIndex));
                    last.DeleteFromValue(last.FirstIndex, end);
                }

                if (cisToModify.Count > 2)
                {
                    var middle = cisToModify.Where(x => x.FirstIndex > start && x.LastIndex < end).ToList();
                    middle.ForEach(x => _contentItems.Remove(x));
                    ShiftBodyIndexes(first.BodyIndex + 1, middle.Count() * -1);
                }

                return;
            }


            throw new ArgumentException("This range of indexes was not found in this text");
        }

        public ContentItem InsertDefinition(
            AddDefinitionRequest request,
            string wordId,
            string annotationId,
            string annotationContextId,
            DbContext context = null
        )
        {
            var startIndex = ContentItems.First(x => x.BodyIndex == request.ContentItemStartIndex).GetGlobalIndex(request.Start);
            var endIndex = ContentItems.First(x => x.BodyIndex == request.ContentItemEndIndex).GetGlobalIndex(request.End);

            return InsertDefinition(startIndex, endIndex, wordId, annotationId, annotationContextId, context);
        }

        public ContentItem InsertDefinition(
            int start,
            int end,
            string wordId,
            string annotationId,
            string annotationContextId,
            DbContext context = null)
        {
            LoadBody(context);
            // a complete individual CI was defined
            if (ContentItems.Any(x => x.FirstIndex == start && x.LastIndex == end))
            {
                var ciToDefine = ContentItems.First(x => x.FirstIndex == start && x.LastIndex == end);
                ciToDefine.WordId = wordId;
                ciToDefine.AnnotationId = annotationId;
                ciToDefine.AnnotationContextId = annotationContextId;
                return ciToDefine;
            }

            // definition occured inside a single CI
            if (ContentItems.Any(x => x.FirstIndex <= start && x.LastIndex >= end))
            {
                var parentCI = ContentItems.First(x => x.FirstIndex <= start && x.LastIndex >= end);
                var ciFactory = new DefinedContentItemFactory(
                                    parentCI.BodyIndex,
                                    parentCI.GetLocalIndex(start),
                                    parentCI.GetLocalIndex(end),
                                    parentCI.Value,
                                    wordId,
                                    annotationId,
                                    annotationContextId
                    );

                var newContentItems = ciFactory.BuildContentItems();

                _contentItems.Remove(parentCI);
                ShiftBodyIndexes(parentCI.BodyIndex, newContentItems.Count - 1);
                _contentItems.AddRange(newContentItems);

                return newContentItems.First(x => x.WordId == wordId);
            }

            // definition occured accross multiple CI
            if (ContentItems.Any(x => x.FirstIndex <= start && x.LastIndex >= end))
            {
                var parentCI = ContentItems.First(x => x.FirstIndex <= start && x.LastIndex >= end);
                var ciFactory = new DefinedContentItemFactory(
                    parentCI.BodyIndex,
                    parentCI.GetLocalIndex(start),
                    parentCI.GetLocalIndex(end),
                    parentCI.Value,
                    wordId,
                    annotationId,
                    annotationContextId
                );

                var newContentItems = ciFactory.BuildContentItems();

                _contentItems.Remove(parentCI);
                ShiftBodyIndexes(parentCI.BodyIndex, newContentItems.Count - 1);

                return newContentItems.First(x => x.WordId == wordId);
            }

            return null;
        }

        public void RemoveDefinition(string contentItemId)
        {
            var ci = ContentItems.FirstOrDefault(x => x.Id == contentItemId);

            if (ci == null)
            {
                _errors.Add($"Could not find Content Item with id {contentItemId}");
                return;
            }

            ci.RemoveDefinition();
            CleanupContentItems();
        }

        public string GetWordContext(AddDefinitionRequest request)
        {
            var startIndex = ContentItems.First(x => x.BodyIndex == request.ContentItemStartIndex).GetGlobalIndex(request.Start);
            var sentenceService = new ExampleSentenceService(Text);
            return sentenceService.GetSentenceAtIndex(startIndex);
        }

        public void CleanupContentItems()
        {
            for (var i = this._contentItems.Count(); i-- > 1;)
            {
                var ci1 = _sortedContentItems[i - 1];
                var ci2 = _sortedContentItems[i];

                if (!ci1.IsDefined() && !ci2.IsDefined())
                {
                    ci1.Value += ci2.Value;
                    _contentItems.Remove(ci2);
                    ShiftBodyIndexes(i, -1);
                }
            }
        }

        #endregion

        #region Private Methods

        // shifts body index of all items starting with item at starting index
        private void ShiftBodyIndexes(int startingIndex, int distance = 1)
        {
            foreach (var item in _contentItems.Where(x => x.BodyIndex >= startingIndex))
            {
                item.BodyIndex += distance;
            }
        }

        private void NullCheckContext(DbContext context, string collection)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context),
                    $"You must provide a context if the {collection} collection isn't valid.");
            }
        }

        private void LoadBody(DbContext context)
        {
            // Because we're using an initialized list,
            // is there another way to do this?

            //if (_contentItems.Any()) return;

            //if (context == null)
            //{
            //    throw new ArgumentNullException(nameof(context),
            //        $"You must provide a context if the body collection isn't valid.");
            //}

            //context.Entry(this).Collection(o => o.ContentItems).Load();
        }

        #endregion
    }
}
