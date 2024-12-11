using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Events.Organization;

namespace VkInfrastructure.EventHandlers
{
    public class OrgAdminAddedHandler : INotificationHandler<OrgAdminAddedEvent>
    {
        public Task Handle(OrgAdminAddedEvent notification, CancellationToken cancellationToken)
        {
            Console.Write("admin added");
            return Task.CompletedTask;
        }
    }
}
