using MediatR;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VkCore.SharedKernel
{
    // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
    public interface IBaseEntity
    {
        string Id { get; set; }
    }

    // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
    public abstract class BaseEntity : IBaseEntity
    {
        private List<INotification> _events = new List<INotification>();
        
        public string Id { get; set; }

        [JsonIgnore]
        public IEnumerable<INotification> Events => _events;

        public void AddEvent(INotification eventItem)
        {
            _events = _events ?? new List<INotification>();
            _events.Add(eventItem);
        }

        public void RemoveEvent(INotification eventItem)
        {
            _events?.Remove(eventItem);
        }

        public void ClearEvents()
        {
            _events = new List<INotification>();
        }
    }
}
