using MediatR;
using System.Collections.Generic;
using VkCore.SharedKernel;

namespace VkCore.Requests.Student
{
    public class GetDistractorListRequest : IRequest<DtoResult<IEnumerable<string>>>
    {

    }
}
