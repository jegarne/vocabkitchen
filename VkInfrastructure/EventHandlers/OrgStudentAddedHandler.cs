using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Events;
using VkCore.Events.Organization;

namespace VkInfrastructure.EventHandlers
{
    public class OrgStudentAddedHandler: INotificationHandler<OrgStudentAddedEvent>
    {
        public Task Handle(OrgStudentAddedEvent notification, CancellationToken cancellationToken)
        {
            Console.Write("student added");
            return Task.CompletedTask;
        }
    }
}
