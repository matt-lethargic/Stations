using Stations.Core.SharedKernel;
using Stations.Core.Stations.Entities;

namespace Stations.Core.Stations.Events
{
    public class StationNameChanged : BaseDomainEvent
    {
        public Station Station { get; }

        public StationNameChanged(Station station)
        {
            Station = station;
        }
    }
}