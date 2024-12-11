using System;
using VkCore.SharedKernel;

namespace VkCore.Models.Invite
{
    public enum OrgInviteType
    {
        Student,
        Teacher
    }

    public class OrgInvite : BaseEntity
    {
        private OrgInvite() { }
        public OrgInvite(string orgId, string email, OrgInviteType type)
        {
            Id = Guid.NewGuid().ToString();
            OrgId = orgId;
            Email = email;
            InviteType = type;
            InviteDate = DateTime.UtcNow;
        }

        public string OrgId { get; set; }
        public string Email { get; set; }
        public OrgInviteType InviteType { get; set; }
        public DateTime InviteDate { get; set; }
        public bool IsBounced { get; set; }

        public bool IsTeacherInvite()
        {
            return this.InviteType == OrgInviteType.Teacher;
        }

        public bool IsStudentInvite()
        {
            return this.InviteType == OrgInviteType.Student;
        }
    }
}
