using System;
using Stations.Core.Interfaces.Commands;

namespace Stations.Core.Stations.Commands
{
    public class ChangeStationLocation : ICommand
    {
        public Guid Id { get; }
        public double Latitude { get; }
        public double Longitude { get; }

        public ChangeStationLocation(Guid id, double latitude, double longitude)
        {
            Id = id;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
