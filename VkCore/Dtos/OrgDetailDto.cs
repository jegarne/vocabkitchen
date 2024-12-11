using System.Collections.Generic;
using System.Linq;
using VkCore.Models.Organization;

namespace VkCore.Dtos
{
    public class OrgDetailDto
    {
        public OrgDetailDto(Org org)
        {
            Id = org.Id;
            Name = org.Name;

            Teachers = new List<OrgTeacherDto>();
            foreach (var teacher in org.Teachers)
            {
                Teachers.Add(new OrgTeacherDto(
                    teacher.TeacherUser, 
                    org.Admins.Select(a => a.AdminUser)));
            }

            Students = new List<OrgStudentDto>();
            foreach (var student in org.Students)
            {
                Students.Add(new OrgStudentDto(student.StudentUser));
            }

        }

        public string Id { get; set; }
        public string Name { get; set; }
        public List<OrgStudentDto> Students { get; set; }
        public List<OrgTeacherDto> Teachers { get; set; }
    }
}
