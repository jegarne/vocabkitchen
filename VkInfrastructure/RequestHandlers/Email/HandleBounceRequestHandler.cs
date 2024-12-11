using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Requests.Email;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Word
{
    class HandleBounceRequestHandler : IRequestHandler<HandleBounceRequest, Unit>
    {
        private readonly VkDbContext _context;

        public HandleBounceRequestHandler(VkDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(HandleBounceRequest request, CancellationToken cancellationToken)
        {
            if (!request.Notification.IsPermanentBounce())
            {
                return new Unit();
            }

            foreach (var email in request.Notification.GetEmails())
            {
                // invites
                var invite = await _context.Invites.FirstOrDefaultAsync(i => i.Email.ToLower() == email.ToLower());
                if (invite != null)
                {
                    invite.IsBounced = true;
                    await _context.SaveChangesAsync();
                    continue;
                }

                // handle user sign ups with bad emails
                var user = await _context.Users
                                .Include(u => u.AdminOrgs)
                                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
                if (user != null && !user.EmailConfirmed)
                {
                    foreach (var adminOrg in user.AdminOrgs)
                    {
                        // only delete org if it unused
                        var org = await _context.Organizations
                                    .Include(o => o.Admins)
                                    .Include(o => o.Readings)
                                    .Include(o => o.Students)
                                    .Include(o => o.Teachers)
                                    .FirstOrDefaultAsync(o => o.Id == adminOrg.OrgId);
                        if (org != null
                            && org.Admins.Count() == 1
                            && org.Teachers.Count() == 1
                            && !org.Readings.Any()
                            )
                        {
                            _context.OrgAdmins.RemoveRange(user.AdminOrgs);
                            _context.OrgTeachers.RemoveRange(user.TeacherOrgs);
                            _context.OrgStudents.RemoveRange(user.StudentOrgs);
                            _context.Organizations.Remove(adminOrg.Org);
                            _context.Users.Remove(user);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }

            return new Unit();
        }
    }
}
