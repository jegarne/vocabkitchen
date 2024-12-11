using System;
using System.Collections.Generic;
using System.Linq;
using VkCore.Models;
using VkCore.Models.Invite;

namespace VkCore.Dtos
{
    public class OrgStudentDto
    {
        public OrgStudentDto(VkUser student)
        {
            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            Email = student.Email;
            IsInvitePending = false;
        }

        public OrgStudentDto(OrgInvite invite)
        {
            Id = invite.Id;
            Email = invite.Email;
            InviteDate = invite.InviteDate;
            IsInvitePending = true;
            IsBounced = invite.IsBounced;
        }

        public string Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; }
        public bool IsInvitePending { get; set; }
        public DateTime InviteDate { get; set; }
        public bool IsBounced { get; set; }

        public static IEnumerable<OrgStudentDto> FromOrgStudents(IEnumerable<VkUser> students)
        {
            List<OrgStudentDto> result = new List<OrgStudentDto>();

            foreach (var student in students)
            {
                result.Add(new OrgStudentDto(student));   
            }

            return result.AsEnumerable();
        }

        public static IEnumerable<OrgStudentDto> FromStudentInvites(IEnumerable<OrgInvite> invites)
        {
            List<OrgStudentDto> result = new List<OrgStudentDto>();

            foreach (var invite in invites)
            {
                result.Add(new OrgStudentDto(invite));
            }

            return result.AsEnumerable();
        }
    }
}
