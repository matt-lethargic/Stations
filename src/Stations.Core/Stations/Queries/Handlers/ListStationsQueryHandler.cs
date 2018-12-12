using System;
using System.Linq;
using System.Threading.Tasks;
using Stations.Core.Interfaces;
using Stations.Core.Interfaces.Queries;
using Stations.Core.Stations.Entities;
using Stations.DataContracts;

namespace Stations.Core.Stations.Queries.Handlers
{
    public class ListStationsQueryHandler : IQueryHandler<ListStations, PagedResultDataView<StationDataView>>
    {
        private readonly IRepository<Station> _stationRepository;

        public ListStationsQueryHandler(IRepository<Station> stationRepository)
        {
            _stationRepository = stationRepository ?? throw new ArgumentNullException(nameof(stationRepository));
        }

        public async Task<PagedResultDataView<StationDataView>> Handle(ListStations query)
        {
            var stations = await _stationRepository.List();

            var result = new PagedResultDataView<StationDataView>
            {
                Total =  stations.Count,
                Results = stations.Select(x => new StationDataView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                })
            };


            return result;
        }
    }
}