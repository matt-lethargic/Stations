using System;
using Newtonsoft.Json;
using Stations.Core.SharedKernel;
using Stations.Core.Stations.Events;

namespace Stations.Core.Stations.Entities
{
    public class Station : BaseEntity
    {
        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public double Latitude { get; private set; }

        [JsonProperty]
        public double Longitude { get; private set; }

        public Station(Guid id, string name)
        {
            Id = id;
            Name = name;

            Events.Add(new StationCreated(this));
        }

        public void ChangeName(string name)
        {
            Name = name;
            Events.Add(new StationNameChanged(this));
        }

        public void ChangeLocation(double lat, double lng)
        {
            Latitude = lat;
            Longitude = lng;

            Events.Add(new StationLocationChanged(this));
        }
    }
}
