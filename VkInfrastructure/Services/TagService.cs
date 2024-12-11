using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VkCore.Interfaces;
using VkCore.Models.TagModel;
using VkInfrastructure.Data;


namespace VkInfrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly VkDbContext _context;
        private readonly IMediator _mediator;

        public TagService(VkDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a default tag if no tags exist, otherwise will return the default tag
        /// Will return null if tags exist but none are default
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<Tag> GetDefaultTagAsync(string orgId)
        {
            // no tags here, so make a default
            if (!_context.Tags.Any(t => t.OrgId == orgId))
            {
                return await CreateDefaultTag(orgId);
            }

            var defaultTag = await _context.Tags.FirstOrDefaultAsync(t => t.OrgId == orgId && t.IsDefault);
            return defaultTag;
        }

        private async Task<Tag> CreateDefaultTag(string orgId)
        {
            var defaultTag = new Tag(orgId, "DefaultTag")
            {
                Id = Guid.NewGuid().ToString(),
                IsDefault = true,
            };
            _context.Tags.Add(defaultTag);

            await _context.SaveChangesAsync();
            return defaultTag;
        }

    }
}
