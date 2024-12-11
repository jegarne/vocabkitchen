using VkCore.Models.ReadingModel;
using VkCore.SharedKernel;

namespace VkCore.Models.TagModel
{
    public class ReadingTag : BaseEntity
    {
        private ReadingTag() { }

        public ReadingTag(string readingId, Tag tag)
        {
            ReadingId = readingId;
            TagId = tag.Id;
            Tag = tag;
        }

        public string ReadingId { get; set; }
        public Reading Reading { get; set; }

        public string TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
