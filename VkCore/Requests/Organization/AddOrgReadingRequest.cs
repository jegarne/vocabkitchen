using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class AddOrgReadingRequest : IRequest<DtoResult<ReadingDto>>
    {
        public AddOrgReadingRequest()
        { }

        public AddOrgReadingRequest(string orgId, string title, string text)
        {
            OrgId = orgId;
            Title = title;
            Text = text;
        }

        public string UserId { get; set; }
        public string OrgId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
