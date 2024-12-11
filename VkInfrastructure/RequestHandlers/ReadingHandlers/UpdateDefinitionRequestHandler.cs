using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VkCore.Requests.ReadingRequest;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.ReadingHandlers
{
    public class UpdateDefinitionRequestHandler : IRequestHandler<UpdateDefinitionRequest, DtoResult<string>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<string> _response;

        public UpdateDefinitionRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<string>();
        }

        public async Task<DtoResult<string>> Handle(UpdateDefinitionRequest request, CancellationToken cancellationToken)
        {
            var annotation = await _context.Annotations.Where(x => x.Id == request.AnnotationId)
                .FirstOrDefaultAsync(cancellationToken);

            if (annotation == null)
            {
                _response.AddError(nameof(request.AnnotationId), 
                    $"Could not find a definition for annotation id of {request.AnnotationId}.");
                return _response;
            }

            annotation.Value = request.Value;
            if (annotation.IsUserCreated())
            {
                annotation.SetUpdatedBy(request.UserId);
            }

            await _context.SaveChangesAsync(cancellationToken);

            _response.SetValue($"Annotation id {request.AnnotationId} was successfully updated.");

            return _response;
        }
    }
}
