using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stations.Core.Interfaces.Commands;
using Stations.Core.Interfaces.Queries;
using Stations.Core.Stations.Commands;
using Stations.Core.Stations.Queries;
using Stations.DataContracts;
using Stations.Web.Models.Stations;

namespace Stations.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryProcessor _queryProcessor;

        public StationsController(ICommandDispatcher commandDispatcher, IQueryProcessor queryProcessor)
        {
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        [HttpGet("{id:Guid}"), ActionName("Get")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetStation(id);
            StationDataView result = await _queryProcessor.Process(query);
            
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var query = new ListStations();
            var result = await _queryProcessor.Process(query);

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateStationModel model)
        {
            var command = new CreateStation(model.Id, model.Name);
            await _commandDispatcher.Dispatch(command);

            return NoContent();
        }

        [HttpPut("{id:Guid}/location")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ChangeStationLocationModel model)
        {
            var command = new ChangeStationLocation(id, model.Latitude, model.Longitude);
            await _commandDispatcher.Dispatch(command);

            return NoContent();
        }
    }
}
