using System.Collections.Generic;
using System.Linq;

namespace VkCore.Dtos
{
    public class UserAccessDto
    {
        public bool IsAdmin => this.AdminOrgIds.Any();
        public bool IsTeacher => this.TeacherOrgIds.Any();
        public bool IsStudent => this.StudentOrgIds.Any();

        public List<string> AdminOrgIds { get; set; }
        public List<string> TeacherOrgIds { get; set; }
        public List<string> StudentOrgIds { get; set; }
    }
}
