using System;
using System.Collections.Generic;
using System.Linq;
using VkCore.Models;
using VkCore.Models.Invite;

namespace VkCore.Dtos
{
    public class OrgTeacherDto
    {
        public OrgTeacherDto(VkUser teacher, IEnumerable<VkUser> admins, string requestingUserId)
        {
            Id = teacher.Id;
            FirstName = teacher.FirstName;
            LastName = teacher.LastName;
            Email = teacher.Email;
            IsAdmin = admins.Any(a => a.Id == teacher.Id);
            IsInvitePending = false;
            IsMe = teacher.Id == requestingUserId;
        }

        public OrgTeacherDto(VkUser teacher, IEnumerable<VkUser> admins)
        {
            Id = teacher.Id;
            FirstName = teacher.FirstName;
            LastName = teacher.LastName;
            Email = teacher.Email;
            IsAdmin = admins.Any(a => a.Id == teacher.Id);
            IsInvitePending = false;
        }


        public OrgTeacherDto(OrgInvite invite)
        {
            Id = invite.Id;
            Email = invite.Email;
            InviteDate = invite.InviteDate;
            IsInvitePending = true;
            IsAdmin = false;
            IsMe = false;
            IsBounced = invite.IsBounced;
        }

        public string Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; }
        public bool IsAdmin { get; set; }
        public bool IsInvitePending { get; set; }
        public DateTime InviteDate { get; set; }
        public bool IsMe { get; set; }
        public bool IsBounced { get; set; }

        public static IEnumerable<OrgTeacherDto> FromOrgTeachers(
            IEnumerable<VkUser> teachers, 
            IEnumerable<VkUser> admins, 
            string requestingUserId)
        {
            List<OrgTeacherDto> result = new List<OrgTeacherDto>();

            foreach (var teacher in teachers)
            {
                result.Add(new OrgTeacherDto(teacher, admins, requestingUserId));   
            }

            return result.AsEnumerable();
        }

        public static IEnumerable<OrgTeacherDto> FromTeacherInvites(IEnumerable<OrgInvite> invites)
        {
            List<OrgTeacherDto> result = new List<OrgTeacherDto>();

            foreach (var invite in invites)
            {
                result.Add(new OrgTeacherDto(invite));
            }

            return result.AsEnumerable();
        }
    }
}
