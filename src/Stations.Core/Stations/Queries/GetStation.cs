using System;
using Stations.Core.Interfaces.Queries;
using Stations.DataContracts;

namespace Stations.Core.Stations.Queries
{
    public class GetStation : IQuery<StationDataView>
    {
        public Guid Id { get; }

        public GetStation(Guid id)
        {
            Id = id;
        }
    }
}
