using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Constants;
using VkCore.Dtos;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class GetWordsProgressRequestHandler : IRequestHandler<GetWordsProgressRequest, DtoResult<IEnumerable<WordAttemptSummaryDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<WordAttemptSummaryDto>> _response = new DtoResult<IEnumerable<WordAttemptSummaryDto>>();

        public GetWordsProgressRequestHandler(VkDbContext context)
        {
            _context = context;
        }

        public async Task<DtoResult<IEnumerable<WordAttemptSummaryDto>>> Handle(GetWordsProgressRequest request, CancellationToken cancellationToken)
        {
            var readingIds = _context.OrgReadings
                .Where(u => u.OrgId == request.OrgId)
                .Select(r => r.ReadingId).ToList();

            if (!readingIds.Any())
            {
                _response.AddError(nameof(request.OrgId), $"Could not find readings for the org with id of {request.OrgId}.");
                return _response;
            }

            var wordIds = _context.ContentItems
                                    .Where(x => readingIds.Contains(x.ReadingId) && x.WordId != null)
                                    .Select(x => x.WordId).ToList();
            if (!wordIds.Any())
            {
                _response.AddError(nameof(request.OrgId), $"Could not find words for the org with id of {request.OrgId}.");
                return _response;
            }

            var result = new List<WordAttemptSummaryDto>();
            foreach (var wordId in wordIds)
            {
                var word = await _context.Words
                    .FirstOrDefaultAsync(w => w.Id == wordId, cancellationToken: cancellationToken);

                if (word == null)
                    continue;

                var allAttempts = _context.StudentWords
                    .Where(w => w.WordEntryId == wordId)
                    .SelectMany(x => x.Attempts).ToList();

                var summary = new WordAttemptSummaryDto()
                {
                    WordEntryId = wordId,
                    Word = word.Word,
                    ClozeAttempts = allAttempts.Count(x => x.AttemptType == WordAttemptTypes.Cloze),
                    ClozeFailures = allAttempts.Count(x => x.AttemptType == WordAttemptTypes.Cloze && x.WasSuccessful == false),
                    SpellingAttempts = allAttempts.Count(x => x.AttemptType == WordAttemptTypes.Spelling),
                    SpellingFailures = allAttempts.Count(x => x.AttemptType == WordAttemptTypes.Spelling && x.WasSuccessful == false),
                    MeaningAttempts = allAttempts.Count(x => x.AttemptType == WordAttemptTypes.Meaning),
                    MeaningFailures = allAttempts.Count(x => x.AttemptType == WordAttemptTypes.Meaning && x.WasSuccessful == false),
                };

                result.Add(summary);
            }

            if (_response.HasErrors()) return _response;
            _response.SetValue(result);

            return _response;
        }
    }
}
