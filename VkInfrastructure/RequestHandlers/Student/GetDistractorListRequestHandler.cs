using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Requests.Student;
using VkCore.SharedKernel;
using VkInfrastructure.Data;
using VkInfrastructure.Extensions;
using VkInfrastructure.Profilers;

namespace VkInfrastructure.RequestHandlers.Student
{
    public class GetDistractorListRequestHandler : IRequestHandler<GetDistractorListRequest, DtoResult<IEnumerable<string>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<string>> _response = new DtoResult<IEnumerable<string>>();

        public GetDistractorListRequestHandler(VkDbContext context)
        {
            _context = context;
        }

        public Task<DtoResult<IEnumerable<string>>> Handle(GetDistractorListRequest request, CancellationToken cancellationToken)
        {
            var cefrList = new CefrListReader()
                .GetLists().SelectMany(l => l.WordList).ToList();

            _response.SetValue(cefrList.PickRandom(30));

            return Task.FromResult(_response);
        }
    }
}

