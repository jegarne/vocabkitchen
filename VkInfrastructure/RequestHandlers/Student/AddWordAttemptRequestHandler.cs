using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Requests.Student;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Student
{
    public class AddWordAttemptRequestHandler : IRequestHandler<AddWordAttemptRequest, DtoResult<StudentWordDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<StudentWordDto> _response = new DtoResult<StudentWordDto>();

        public AddWordAttemptRequestHandler(VkDbContext context)
        {
            _context = context;
        }

        public async Task<DtoResult<StudentWordDto>> Handle(AddWordAttemptRequest request, CancellationToken cancellationToken)
        {
            var studentWord = await _context.StudentWords
                   .FirstOrDefaultAsync(c => c.VkUserId == request.UserId
                   && c.WordEntryId == request.WordId
                   && c.AnnotationId == request.AnnotationId, cancellationToken);

            if (studentWord == null)
            {
                _response.AddError(nameof(request.WordId),
                    $"Could not find a student word for word {request.WordId} and student {request.UserId}");
                return _response;
            }

            studentWord.AddAttempt(request.AttemptType, request.WasSuccessful, _context);
            _context.SaveChanges();

            await studentWord.Hydrate(_context);
            _response.SetValue(new StudentWordDto(studentWord));

            return _response;
        }
    }
}

