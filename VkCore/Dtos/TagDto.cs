using System.Collections.Generic;
using VkCore.Models.TagModel;

namespace VkCore.Dtos
{
    public class TagDto
    {
        public TagDto() { }

        public TagDto(string id, string value, bool isDefault)
        {
            Id = id;
            Value = value;
            IsDefault = isDefault;
        }

        public static IEnumerable<TagDto> BuildTagDtos(IEnumerable<Tag> tags)
        {
            var result = new List<TagDto>();

            foreach (var tag in tags)
            {
                result.Add(new TagDto(tag.Id, tag.Value, tag.IsDefault));
            }

            return result;
        }

        public static IEnumerable<TagDto> BuildTagDtos(IEnumerable<UserTag> tags)
        {
            var result = new List<TagDto>();

            if (tags == null) return result;

            foreach (var tag in tags)
            {
                result.Add(new TagDto(tag.Tag.Id, tag.Tag.Value, tag.Tag.IsDefault));
            }

            return result;
        }

        public static IEnumerable<TagDto> BuildTagDtos(IEnumerable<ReadingTag> tags)
        {
            var result = new List<TagDto>();

            foreach (var tag in tags)
            {
                result.Add(new TagDto(tag.Tag.Id, tag.Tag.Value, tag.Tag.IsDefault));
            }

            return result;
        }

        public string Id { get; set; }
        public string OrgId { get; set; }
        public string Value { get; set; }
        public bool IsDefault { get; set; }
    }
}
