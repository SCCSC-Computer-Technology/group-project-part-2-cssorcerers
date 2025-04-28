using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsAPI.Interfaces;
using SportsData.Models;

namespace SportsAPI.Controllers
{
    [Route("api/f1")]
    [ApiController]
    public class F1ApiController : ControllerBase
    {
        private readonly IF1Repository repo;

        public F1ApiController(IF1Repository repo)
        {
            this.repo = repo;
        }

        [HttpGet("constructors", Name = nameof(GetF1Constuctors))]
        [ProducesResponseType(200, Type = typeof(List<F1Constructor>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<F1Constructor>>> GetF1Constuctors(string? sort, int page)
        {
            if (page < 1) page = 1;
            var data = await repo.RetreiveAllConstructorInfoAsync(sort, page);
            if (data == null) return NotFound();
            return data;
        }

        [HttpGet("constructors/{id}", Name = nameof(GetF1Constuctor))]
        [ProducesResponseType(200, Type = typeof(F1Constructor))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<F1Constructor>> GetF1Constuctor(int id)
        {
            var data = await repo.RetreiveConstructorInfoAsync(id);
            if (data == null) return NotFound();
            return data;
        }
        [HttpGet("constructors/count", Name = nameof(GetF1ConstructorsCount))]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<ActionResult<int>> GetF1ConstructorsCount()
        {
            return await repo.RetrieveConstructorCountAsync();
        }

        [HttpGet("currentconstructors", Name = nameof(GetF1CurrentConstuctors))]
        [ProducesResponseType(200, Type = typeof(List<F1CurrentConstructor>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<F1CurrentConstructor>>> GetF1CurrentConstuctors(int page)
        {
            if ( page < 1) page = 1;
            var data = await repo.RetreiveAllCurrentConstructorInfoAsync(page);
            if (data == null) return NotFound();
            return data;
        }

        [HttpGet("currentconstructors/{id}", Name = nameof(GetF1CurrentConstuctor))]
        [ProducesResponseType(200, Type = typeof(F1CurrentConstructor))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<F1CurrentConstructor>> GetF1CurrentConstuctor(int id)
        {
            var data = await repo.RetreiveCurrentConstructorInfoAsync(id);
            if (data == null) return NotFound();
            return data;
        }

        [HttpGet("currentconstructors/count", Name = nameof(GetF1CurrentConstructorsCount))]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<ActionResult<int>> GetF1CurrentConstructorsCount()
        {
            return await repo.RetrieveCurrentConstructorCountAsync();
        }

        [HttpGet("drivers/{id}", Name = nameof(GetF1Driver))]
        [ProducesResponseType(200, Type = typeof(F1DriverInfo))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<F1DriverInfo>> GetF1Driver(int id)
        {
            var data = await repo.RetreiveDriverInfoAsync(id);
            if (data == null) return NotFound();
            return data;
        }
        [HttpGet("drivers", Name = nameof(GetF1Drivers))]
        [ProducesResponseType(200, Type = typeof(List<F1DriverInfo>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<F1DriverInfo>>> GetF1Drivers(string? sort, int page)
        {
            var data = await repo.RetreiveAllDriversInfoAsync(sort, page);
            if (data == null || !data.Any()) return NotFound();
            return data;
        }
        [HttpGet("drivers/count", Name = nameof(GetF1DriversCount))]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<ActionResult<int>> GetF1DriversCount()
        {
            return await repo.RetrieveDriverCountAsync();
        }

        [HttpGet("races", Name = nameof(GetF1Races))]
        [ProducesResponseType(200, Type = typeof(List<F1RaceInfo>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<F1RaceInfo>>> GetF1Races(int page)
        {
            if (page < 1) page = 1;
            var data = await repo.RetreiveAllRaceInfoAsync(page);
            if (data == null) return NotFound();
            return data;
        }

        [HttpGet("races/{id}", Name = nameof(GetF1Race))]
        [ProducesResponseType(200, Type = typeof(F1RaceInfo))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<F1RaceInfo>> GetF1Race(int id)
        {
            var data = await repo.RetreiveRaceInfoAsync(id);
            if (data == null) return NotFound();
            return data;
        }

        [HttpGet("races/count", Name = nameof(GetF1RacesCount))]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<ActionResult<int>> GetF1RacesCount()
        {
            return await repo.RetrieveRaceCountAsync();
        }
    }
}
