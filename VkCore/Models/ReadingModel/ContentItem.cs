using System;
using System.Collections.Generic;
using VkCore.Dtos;
using VkCore.Models.Word;
using VkCore.SharedKernel;

namespace VkCore.Models.ReadingModel
{
    public class ContentItem : BaseEntity
    {
        private int _firstIndex;
        private string _value;

        public string ReadingId { get; set; }

        public ContentItem(
            int bodyIndex, 
            string value, 
            string wordId = null,
            string annotationId = null,
            string annotationContextId = null
            )
        {
            //if (string.IsNullOrWhiteSpace(value))
            //    throw new ArgumentException("A content item cannot contain an empty value.");

            Id = Guid.NewGuid().ToString();
            BodyIndex = bodyIndex;
            Value = value;
            WordId = wordId;
            AnnotationId = annotationId;
            AnnotationContextId = annotationContextId;
        }

        public int BodyIndex { get; set; }

        // Example:
        // hello
        // 01234
        // StartIndex = 0
        // EndIndex = 5

        public int FirstIndex => _firstIndex;
        public int LastIndex => _firstIndex + Value.Length - 1;

        public string WordId { get; set; }
        public WordEntry Word { get; set; }

        public string AnnotationId { get; set; }
        public Annotation Annotation { get; set; }

        public string AnnotationContextId { get; set; }

        public string Value
        {
            get { return _value.Replace("\r\n", "\n"); }
            set { _value = value; }
        }

        public bool IsDefined()
        {
            return this.WordId != null;
        }

        public void SetFirstIndex(int firstIndex)
        {
            _firstIndex = firstIndex;
        }

        public int GetLocalIndex(int globalIndex)
        {
            return globalIndex == 0 ? 0 : globalIndex - FirstIndex;
        }

        public int GetGlobalIndex(int localIndex)
        {
            return localIndex + _firstIndex;
        }

        public void DeleteFromValue(int startIndex, int endIndex)
        {
            int localStartIndex = GetLocalIndex(startIndex);
            int localEndIndex = GetLocalIndex(endIndex);

            string left = "";
            string right = "";

            if (localStartIndex != 0)
                left = Value.Substring(0, localStartIndex);

            if (localEndIndex != Value.Length - 1)
                right = Value.Substring(localEndIndex + 1, Value.Length - localEndIndex - 1);

            RemoveDefinition();
            Value = left + right;
        }

        public List<ContentItem> SplitOutNewValue(int insertIndex, string newValue)
        {
            var list = new List<ContentItem>();

            var localInsertIndex = GetLocalIndex(insertIndex);

            // split this value at insert index and update this value to first half
            var newSecondHalf = this.Value.Substring(localInsertIndex);
            this.Value = this.Value.Remove(localInsertIndex);

            // create a new content item at the end of the first half and the length of the new value
            list.Add(new ContentItem(this.BodyIndex + 1, newValue));

            // create a new content item with the value of the second half
            list.Add(new ContentItem(this.BodyIndex + 2, newSecondHalf));

            RemoveDefinition();
            return list;
        }

        public List<ContentItem> InsertValue(int insertIndex, string newValue)
        {
            var list = new List<ContentItem>();

            var localInsertIndex = GetLocalIndex(insertIndex);
            this.Value = this.Value.Insert(localInsertIndex, newValue);

            RemoveDefinition();
            return list;
        }

        public void RemoveDefinition()
        {
            this.AnnotationId = null;
            this.WordId = null;
            this.AnnotationContextId = null;
        }
    }
}
