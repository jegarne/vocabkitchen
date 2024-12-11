using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VkCore.Dtos;
using VkCore.Models;
using VkCore.Requests.ReadingRequest;
using VkCore.SharedKernel;
using VkInfrastructure.Data;
using VkInfrastructure.Extensions;

namespace VkInfrastructure.RequestHandlers.ReadingHandlers
{
    public class AddProfilerReadingRequestHandler : IRequestHandler<AddProfilerReadingRequest, DtoResult<ReadingDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<ReadingDto> _response;

        public AddProfilerReadingRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<ReadingDto>();
        }

        public async Task<DtoResult<ReadingDto>> Handle(AddProfilerReadingRequest request, CancellationToken cancellationToken)
        {
            var user = _context.Find<VkUser>(request.UserId);
            if (user == null)
            {
                _response.AddError(nameof(request.UserId), $"Could not find the user with id of {request.UserId}.");
                return _response;
            }

            var defaultOrgTeacher = _context.OrgTeachers.FirstOrDefault(o => o.VkUserId == request.UserId && o.IsDefault);
            if (defaultOrgTeacher == null)
            {
                _response.AddError(nameof(request.UserId), $"Could not find a default organization for User {request.UserId}.");
                return _response;
            }

            var org = _context.Organizations.Find(defaultOrgTeacher.OrgId);

            if (org == null)
            {
                _response.AddError(nameof(request.UserId), $"Could not find the default organization for user {request.UserId}.");
                return _response;
            }

            var newReading = new VkCore.Models.ReadingModel.Reading(request.Text.FirstWords(6) + "...", request.Text);
            org.AddReading(newReading, _context);

            _response.AddErrors(org);
            if (_response.HasErrors()) return _response;

            await _context.SaveChangesAsync(cancellationToken);

            var readingDto = new ReadingDto(newReading);
            readingDto.OrgId = org.Id;

            _response.SetValue(readingDto);

            return _response;
        }
    }
}
