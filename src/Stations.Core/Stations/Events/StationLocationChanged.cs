using Stations.Core.SharedKernel;
using Stations.Core.Stations.Entities;

namespace Stations.Core.Stations.Events
{
    public class StationLocationChanged : BaseDomainEvent
    {
        public Station Station { get; }

        public StationLocationChanged(Station station)
        {
            Station = station;
        }
    }
}
