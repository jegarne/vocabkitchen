using MediatR;
using VkCore.Dtos;

namespace VkCore.Requests.Email
{
    public class HandleBounceRequest : IRequest
    {
        public HandleBounceRequest(AmazonSesBounceNotification notification)
        {
            Notification = notification;
        }

        public AmazonSesBounceNotification Notification { get; set; }
    }
}
