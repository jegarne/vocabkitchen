﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Models.Word;
using VkCore.Requests.Student;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Student
{
    public class AddStudentWordRequestHandler : IRequestHandler<AddStudentWordRequest, DtoResult<StudentWordDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<StudentWordDto> _response = new DtoResult<StudentWordDto>();

        public AddStudentWordRequestHandler(VkDbContext context)
        {
            _context = context;
        }

        public async Task<DtoResult<StudentWordDto>> Handle(AddStudentWordRequest request, CancellationToken cancellationToken)
        {
            var studentWord = await _context.StudentWords
                   .FirstOrDefaultAsync(c => c.VkUserId == request.UserId
                   && c.WordEntryId == request.WordId
                   && c.AnnotationId == request.AnnotationId);

            if (studentWord != null)
            {
                _response.AddError(nameof(request.WordId),
                    $"Word {request.WordId} is already associated with student {request.UserId}");
                return _response;
            }

            var word = await _context.Words
                .Include(w => w.Annotations)
                .FirstOrDefaultAsync(w => w.Id == request.WordId
                && w.Annotations.Any(a => a.Id == request.AnnotationId), cancellationToken);

            if (word == null)
            {
                _response.AddError(nameof(request.WordId), $"Could not find a word with the id of {request.WordId}.");
                return _response;
            }

            studentWord = new StudentWord(request.UserId, request.WordId, request.AnnotationId);
            _context.Add(studentWord);
            await _context.SaveChangesAsync();

            await studentWord.Hydrate(_context);
            _response.SetValue(new StudentWordDto(studentWord));

            return _response;
        }
    }
}

