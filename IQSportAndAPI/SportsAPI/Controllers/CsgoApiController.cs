using Microsoft.AspNetCore.Mvc;
using SportsAPI.Interfaces;
using SportsData.Models;

namespace SportsAPI.Controllers
{
    [Route("api/csgo")]
    [ApiController]
    public class CsgoApiController : ControllerBase
    {
        private readonly ICSGORepository repo;

        public CsgoApiController(ICSGORepository repo)
        {
            this.repo = repo;
        }

        [HttpGet("players", Name = nameof(GetCSGOPlayers))]
        [ProducesResponseType(200, Type = typeof(List<CSGOPlayer>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<CSGOPlayer>>> GetCSGOPlayers(string? name, double? minRating, int? minMaps, int? page, int? pageSize, string? sort, bool isDesc)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            if(!pageSize.HasValue || pageSize.Value < 1) pageSize = 10;
            var data = await repo.RetrievePlayers(name, minRating, minMaps, page.Value, pageSize.Value, sort, isDesc);
            if (data == null || !data.Any()) return NotFound();
            return data;

        }

        [HttpGet("players/count", Name = nameof(GetCSGOPlayerCount))]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<ActionResult<int>> GetCSGOPlayerCount(string? name, double? minRating, int? minMaps)
        {
            return await repo.RetrievePlayerCount(name, minRating, minMaps);
        }
    }
}
