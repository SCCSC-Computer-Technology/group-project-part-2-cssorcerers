using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsAPI.Interfaces;
using SportsData;
using SportsData.Models;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace SportsAPI.Controllers
{
    [Route("api/nba")]
    [ApiController]
    public class NbaApiController : ControllerBase
    {
        private readonly INBARepository repo;

        public NbaApiController(INBARepository repo)
        {
            this.repo = repo;
        }
        //team actions
        [HttpGet("teams/avg", Name = nameof(GetNBATeamAverages))]
        [ProducesResponseType(200, Type = typeof(List<NBATeamSeasonInfo>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<NBATeamSeasonInfo>>> GetNBATeamAverages(string? sort, int? page)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            var data = await repo.RetrieveTeamAveragesAsync(sort, page.Value);
            if (data == null || !data.Any()) return NotFound();
            return data;
        }

        [HttpGet("teams/avg/{id}", Name = nameof(GetNBATeamAverage))]
        [ProducesResponseType(200, Type = typeof(NBATeamSeasonInfo))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NBATeamSeasonInfo>> GetNBATeamAverage(int id)
        {
            var data = await repo.RetrieveTeamAverageAsync(id);
            if (data == null) return NotFound();
            return data;
        }

        [HttpGet("teams", Name = nameof(GetNBATeams))]
        [ProducesResponseType(200, Type = typeof(List<NBATeamSeasonInfo>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<NBATeamSeasonInfo>>> GetNBATeams(string? sort, int? page)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            var data = await repo.RetrieveAllTeamInfoAsync(sort, page.Value);
            if (data == null || !data.Any()) return NotFound();
            return data;
        }

        [HttpGet("teams/season/{season}", Name = nameof(GetNBATeamsBySeason))]
        [ProducesResponseType(200, Type = typeof(List<NBATeamSeasonInfo>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<NBATeamSeasonInfo>>> GetNBATeamsBySeason(string? sort, int season, int? page)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            var data = await repo.RetrieveTeamInfoBySeasonAsync(sort, season, page.Value);
            if (data == null || !data.Any()) return NotFound();
            return data;
        }

        [HttpGet("teams/{id}", Name = nameof(GetNBATeamByID))]
        [ProducesResponseType(200, Type = typeof(List<NBATeamSeasonInfo>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<NBATeamSeasonInfo>>> GetNBATeamByID(string? sort, int id, int? page)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            var data = await repo.RetrieveTeamInfoByIDAsync(sort, id, page.Value);
            if (data == null || !data.Any()) return NotFound();
            return data;
        }

        [HttpGet("teams/{id}/{season}", Name = nameof(GetNBATeamBySeason))]
        [ProducesResponseType(200, Type = typeof(NBATeamSeasonInfo))]
        public async Task<ActionResult<NBATeamSeasonInfo>> GetNBATeamBySeason(int id, int season)
        {
            var data = await repo.RetrieveTeamSeasonInfoAsync(id, season);
            if (data == null) return NotFound();
            return data;
        }


        //player actions
        [HttpGet("players", Name = nameof(GetNBAPlayers))]
        [ProducesResponseType(200, Type = typeof(List<NBAPlayer>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<NBAPlayer>>> GetNBAPlayers(string? sort, bool? isActive, int? page)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            if (isActive != null)
            {
                var data = await repo.RetrievePlayerInfoByStatusAsync(sort, isActive.Value, page.Value);
                if (data == null || !data.Any()) return NotFound();
                return data;
            }
            else
            {
                var data = await repo.RetrieveAllPlayerInfoAsync(sort, page.Value);
                if (data == null || !data.Any()) return NotFound();
                return data;
            }
        }

        [HttpGet("players/{id}", Name = nameof(GetNBAPlayer))]
        [ProducesResponseType(200, Type = typeof(NBAPlayer))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NBAPlayer>> GetNBAPlayer(int id)
        {
            var data = await repo.RetrievePlayerInfoByIDAsync(id);
            if (data == null) return NotFound();
            return data;
        }

        [HttpGet("players/team/{id}", Name = nameof(GetNBAPlayersByTeam))]
        [ProducesResponseType(200, Type = typeof(List<NBAPlayer>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<NBAPlayer>>> GetNBAPlayersByTeam(string? sort, int id, int? page)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            var data = await repo.RetrievePlayerInfoByTeamAsync(sort, id, page.Value);
            if (data == null || !data.Any()) return NotFound();
            return data;
        }

        [HttpGet("SeasonMax", Name = nameof(GetNBAMaxSeason))]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> GetNBAMaxSeason()
        {
            return await repo.RetrieveMaxSeasonAsync();
        }

        [HttpGet("SeasonMin", Name = nameof(GetNBAMinSeason))]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> GetNBAMinSeason()
        {
            return await repo.RetrieveMinSeasonAsync();
        }

        [HttpGet("teams/count", Name = nameof(GetTeamStatCount))]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> GetTeamStatCount(int? season, int? teamID)
        {
            return await repo.RetrieveTeamCountAsync(season, teamID);
        }

        [HttpGet("players/count", Name = nameof(GetNBAPlayerCount))]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> GetNBAPlayerCount(int? teamID, bool? isActive)
        {
            return await repo.RetrievePlayerCountAsync(teamID, isActive);
        }
    }
}
