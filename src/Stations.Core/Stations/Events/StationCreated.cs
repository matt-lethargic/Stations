using Stations.Core.SharedKernel;
using Stations.Core.Stations.Entities;

namespace Stations.Core.Stations.Events
{
    public class StationCreated : BaseDomainEvent
    {
        public Station Station { get; }

        public StationCreated(Station station)
        {
            Station = station;
        }
    }
}
