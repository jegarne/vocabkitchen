using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VkCore.Events.User;
using VkCore.Interfaces;
using VkCore.Requests.User;

namespace VkInfrastructure.EventHandlers
{
    public class NewUserAddedConfirmEmailHandler : INotificationHandler<NewUserAddedEvent>
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;

        public NewUserAddedConfirmEmailHandler(
            IMediator mediator,
            IUserService userService
        )
        {
            _mediator = mediator;
            _userService = userService;
        }

        public async Task Handle(NewUserAddedEvent notification, CancellationToken cancellationToken)
        {
            if (notification.IsEmailConfirmed)
            {
                var user = await _userService.FindByIdAsync(notification.User.Id);
                var token = await _userService.GenerateEmailConfirmationTokenAsync(user);
                var identityResult = await _userService.ConfirmEmailAsync(user, token);
            }
            else
            {
                await _mediator.Send(new SendConfirmationEmailRequest(notification.User.Email));
            }
        }
    }
}
