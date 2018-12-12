using System;
using System.Threading.Tasks;
using Stations.Core.Interfaces;
using Stations.Core.Interfaces.Queries;
using Stations.Core.Stations.Entities;
using Stations.DataContracts;

namespace Stations.Core.Stations.Queries.Handlers
{
    public class GetStationQueryHandler : IQueryHandler<GetStation, StationDataView>
    {
        private readonly IRepository<Station> _stationRepository;

        public GetStationQueryHandler(IRepository<Station> stationRepository)
        {
            _stationRepository = stationRepository ?? throw new ArgumentNullException(nameof(stationRepository));
        }

        public async Task<StationDataView> Handle(GetStation query)
        {
            var station = await _stationRepository.GetById(query.Id);

            return new StationDataView
            {
                Id = station.Id,
                Name = station.Name,
                Latitude = station.Latitude,
                Longitude = station.Longitude
            };
        }
    }
}
