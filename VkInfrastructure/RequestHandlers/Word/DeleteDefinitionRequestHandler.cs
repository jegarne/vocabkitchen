using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Builders;
using VkCore.Dtos;
using VkCore.Requests.Word;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Word
{
    class DeleteDefinitionRequestHandler : IRequestHandler<DeleteDefinitionRequest, DtoResult<DefinitionDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<DefinitionDto> _response;

        public DeleteDefinitionRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<DefinitionDto>();
        }

        public async Task<DtoResult<DefinitionDto>> Handle(DeleteDefinitionRequest request, CancellationToken cancellationToken)
        {
            var definition = await _context.Annotations.FindAsync(request.AnnotationId);

            if (definition == null)
            {
                _response.AddError(nameof(request.AnnotationId), $"Could not find the definition with id of {request.AnnotationId}.");
                return _response;
            }

            if (definition.UpdatedBy != request.RequestingUserId)
            {
                _response.AddError(nameof(request.AnnotationId), $"A definition can only be deleted by the user who last updated it.");
                return _response;
            }

            _context.Remove(definition);
            await _context.SaveChangesAsync(cancellationToken);

            _response.SetValue(DefinitionDtoBuilder.FromAnnotation(definition, request.RequestingUserId));

            return _response;
        }
    }
}
