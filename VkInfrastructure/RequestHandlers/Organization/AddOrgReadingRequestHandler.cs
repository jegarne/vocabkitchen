using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class AddOrgReadingRequestHandler : IRequestHandler<AddOrgReadingRequest, DtoResult<ReadingDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<ReadingDto> _response;
        private readonly ITagService _tagService;

        public AddOrgReadingRequestHandler(
            VkDbContext context, 
            IMediator mediator,
            ITagService tagService
        ) {
            _context = context;
            _response = new DtoResult<ReadingDto>();
            _tagService = tagService;
        }

        public async Task<DtoResult<ReadingDto>> Handle(AddOrgReadingRequest request, CancellationToken cancellationToken)
        {
            var org = _context.Organizations.Find(request.OrgId);

            if (org == null)
            {
                _response.AddError(nameof(request.OrgId), $"Could not find the organization with id of {request.OrgId}.");
                return _response;
            }

            var user = _context.Find<VkUser>(request.UserId);
            if (user == null)
            {
                _response.AddError(nameof(request.UserId), $"Could not find the user with id of {request.UserId}.");
                return _response;
            }
            
            var newReading = new VkCore.Models.ReadingModel.Reading(request.Title, request.Text);
            org.AddReading(newReading, _context);

            var defaultTag = await _tagService.GetDefaultTagAsync(request.OrgId);
            defaultTag?.AddReading(newReading.Id, _context);

            _response.AddErrors(org);
            if (_response.HasErrors()) return _response;

            await _context.SaveChangesAsync(cancellationToken);

            _response.SetValue(new ReadingDto(newReading));

            return _response;
        }
    }
}
