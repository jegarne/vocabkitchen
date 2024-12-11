using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using VkCore.Models.Organization;
using VkCore.Models.TagModel;
using VkCore.Models.Word;
using VkCore.SharedKernel;

namespace VkCore.Models
{
    public class VkUser : IdentityUser, IBaseEntity
    {
        private VkUser() { }
        public VkUser(string email, string firstName, string lastName)
        {
            Email = email;
            UserName = email;
            FirstName = firstName;
            LastName = lastName;
        }

        // Extended Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? FacebookId { get; set; }
        public string PictureUrl { get; set; }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        public List<OrgAdmin> AdminOrgs { get; } = new List<OrgAdmin>();
        public List<OrgTeacher> TeacherOrgs { get; } = new List<OrgTeacher>();
        public List<OrgStudent> StudentOrgs { get; } = new List<OrgStudent>();
        public List<OrgReading> Readings { get; } = new List<OrgReading>();

        public List<UserTag> UserTags { get; } = new List<UserTag>();
        public List<StudentWord> UserWords { get; } = new List<StudentWord>();
    }
}
