using System.Collections.Generic;

namespace VkCore.Models.ReadingModel
{
    public class ContentItemDescription
    {
        public int StartBodyIndex;
        public int StartTextIndex;
        public int Length;
        public string WordId;
        public string AnnotationId { get; set; }
        public string AnnotationContextId { get; set; }

        public ContentItemDescription(
            int startBodyIndex,
            int startTextIndex,
            int length,
            string wordId = null,
            string annotationId = null,
            string contextId = null
            )
        {
            StartBodyIndex = startBodyIndex;
            StartTextIndex = startTextIndex;
            Length = length;
            WordId = wordId;
            AnnotationId = annotationId;
            AnnotationContextId = contextId;
        }
    }

    public class DefinedContentItemFactory
    {
        private readonly int _bodyIndexStart;
        private readonly int _start;
        private readonly int _end;
        private readonly string _baseText;
        private readonly string _wordId;
        private readonly string _annotationId;
        private readonly string _contextId;
        private readonly List<ContentItemDescription> _descriptions = new List<ContentItemDescription>();

        public DefinedContentItemFactory(
            int bodyIndexStart,
            int start,
            int end,
            string baseText,
            string wordId = null,
            string annotationId = null,
            string contextId = null
            )
        {
            this._bodyIndexStart = bodyIndexStart;
            this._start = start;
            this._end = end;
            this._baseText = baseText;
            this._wordId = wordId;
            this._annotationId = annotationId;
            this._contextId = contextId;
            BuildContentItemDescriptions();
        }

        private void BuildContentItemDescriptions()
        {
            if (_start == 0 && _end == _baseText.Length - 1)
            {
                _descriptions.Add(new ContentItemDescription(_bodyIndexStart, 0, _baseText.Length, _wordId, _annotationId, _contextId));
                return;
            }

            if (_start == 0)
            {
                _descriptions.Add(new ContentItemDescription(_bodyIndexStart, 0, _end + 1, _wordId, _annotationId, _contextId));
                _descriptions.Add(new ContentItemDescription(_bodyIndexStart + 1, _end + 1, _baseText.Length - _end - 1));
                return;
            }

            if (_end == _baseText.Length - 1)
            {
                _descriptions.Add(new ContentItemDescription(_bodyIndexStart, 0, _start));
                _descriptions.Add(new ContentItemDescription(_bodyIndexStart + 1, _start, _baseText.Length - _start, _wordId, _annotationId, _contextId));
                return;
            }

            _descriptions.Add(new ContentItemDescription(_bodyIndexStart, 0, _start));
            _descriptions.Add(new ContentItemDescription(_bodyIndexStart + 1, _start, _end - _start + 1, _wordId, _annotationId, _contextId));
            _descriptions.Add(new ContentItemDescription(_bodyIndexStart + 2, _end + 1, _baseText.Length - _end - 1));
        }

        public List<ContentItem> BuildContentItems()
        {
            var result = new List<ContentItem>();

            foreach (var d in _descriptions)
            {
                var text = _baseText.Substring(d.StartTextIndex, d.Length);

                result.Add(new ContentItem(
                    d.StartBodyIndex,
                    text,
                    d.WordId,
                    d.AnnotationId,
                    d.AnnotationContextId
                ));
            }

            return result;
        }
    }
}
