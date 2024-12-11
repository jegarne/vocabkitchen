using System.Collections.Generic;
using System.Linq;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Models.TagModel;

namespace VkCore.Dtos
{
    public class StudentDto
    {
        public StudentDto(VkUser user, IEnumerable<TagDto> tags = null)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            FacebookId = user.FacebookId;
            PictureUrl = user.PictureUrl;
            Tags = tags?.ToList();
        }

        public static IEnumerable<StudentDto> BuildStudentDtos(IEnumerable<VkUser> users)
        {
            var result = new List<StudentDto>();

            foreach (var user in users)
            {
                result.Add(new StudentDto(user, TagDto.BuildTagDtos(user.UserTags)));
            }


            return result;
        }

        public static IEnumerable<StudentDto> BuildStudentDtos(IEnumerable<UserTag> users)
        {
            var result = new List<StudentDto>();

            foreach (var user in users)
            {
                result.Add(new StudentDto(user.User, TagDto.BuildTagDtos(user.User.UserTags)));
            }


            return result;
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? FacebookId { get; set; }
        public string PictureUrl { get; set; }

        public void SetProgressCounts(IStudentDataService service)
        {
            this.KnownWordsCount = service.GetKnownWordsCount();
            this.InProgressWordsCount = service.GetInProgressWordsCount();
            this.NewReadingsCount = service.GetNewReadingsCount();
            this.NewWordsCount = service.GetNewWordsCount();
        }

        public int KnownWordsCount { get; set; }
        public int InProgressWordsCount { get; set; }
        public int NewWordsCount { get; set; }
        public int NewReadingsCount { get; set; }

        public List<TagDto> Tags { get; set; }
    }
}
