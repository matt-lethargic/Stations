using System.Threading.Tasks;
using Stations.Core.Interfaces.Events;

namespace Stations.Core.Stations.Events.Handlers
{
    public class StationLocationChangedHandler : IDomainEventHandler<StationLocationChanged>
    {
        public Task Handle(StationLocationChanged domainEvent)
        {
            // Do nothing today
            return Task.CompletedTask;
        }
    }
}
