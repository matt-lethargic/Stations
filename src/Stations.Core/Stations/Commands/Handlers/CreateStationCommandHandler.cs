using System;
using System.Threading.Tasks;
using Stations.Core.Interfaces;
using Stations.Core.Interfaces.Commands;
using Stations.Core.Stations.Entities;

namespace Stations.Core.Stations.Commands.Handlers
{
    public class CreateStationCommandHandler : ICommandHandler<CreateStation>
    {
        private readonly IRepository<Station> _stationRepository;

        public CreateStationCommandHandler(IRepository<Station> stationRepository)
        {
            _stationRepository = stationRepository ?? throw new ArgumentNullException(nameof(stationRepository));
        }
        public async Task Handle(CreateStation command)
        {
            var station = new Station(command.Id, command.Name);
            await _stationRepository.Save(station);
        }
    }
}
