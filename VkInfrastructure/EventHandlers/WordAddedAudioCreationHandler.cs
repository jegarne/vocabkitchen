using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Events.Word;
using VkCore.Requests.Word;

namespace VkInfrastructure.EventHandlers
{
    public class WordAddedAudioCreationHandler : INotificationHandler<WordAddedEvent>
    {
        private readonly IMediator _mediator;

        public WordAddedAudioCreationHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(WordAddedEvent notification, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CreateWordAudioRequest(notification.Word), cancellationToken);
        }
    }
}
