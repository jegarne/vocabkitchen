using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VkCore.Constants;
using VkInfrastructure.Data;
using VkWeb.Extensions;

namespace VkWeb.AuthorizationHandlers
{

    public class UserTypeHandler : AuthorizationHandler<UserTypeRequirement, string>
    {
        VkDbContext _dbContext;

        public UserTypeHandler(VkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            UserTypeRequirement requirement,
            string orgId
            )
        {
            if (requirement.UserType == UserTypes.Admin)
            {
                if (await _dbContext.Organizations
                    .AnyAsync(x => x.Id == orgId && x.Admins.Any(u => u.VkUserId == context.User.GetId())))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
