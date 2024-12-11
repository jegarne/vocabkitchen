using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Events;
using VkCore.Events.Organization;

namespace VkInfrastructure.EventHandlers
{
    public class OrgTeacherAddedHandler : INotificationHandler<OrgTeacherAddedEvent>
    {
        public Task Handle(OrgTeacherAddedEvent notification, CancellationToken cancellationToken)
        {
            Console.Write("teacher added");
            return Task.CompletedTask; ;
        }
    }
}
