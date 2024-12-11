using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Requests.TagRequest;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.TagHandlers
{
    public class GetTagRequestHandler : IRequestHandler<GetTagRequest, DtoResult<TagDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<TagDto> _response;

        public GetTagRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<TagDto>();
        }

        public async Task<DtoResult<TagDto>> Handle(GetTagRequest request, CancellationToken cancellationToken)
        {
            var tag = await _context.Tags.FindAsync(request.TagId);

            if (tag == null)
            {
                _response.AddError(nameof(request.TagId), $"Could not find the tag with id of {request.TagId}.");
                return _response;
            }

            _response.SetValue(new TagDto(tag.Id, tag.Value, tag.IsDefault));

            return _response;
        }
    }
}
