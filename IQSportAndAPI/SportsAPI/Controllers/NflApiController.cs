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
    [Route("api/nfl")]
    [ApiController]
    public class NflApiController : ControllerBase
    {
        private readonly INFLRepository repo;

        public NflApiController(INFLRepository repo)
        {
            this.repo = repo;
        }
        //team actions
        [HttpGet("teams/avg", Name = nameof(GetTeamAverages))]
        [ProducesResponseType(200, Type = typeof(List<NFLTeamSeasonInfo>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<NFLTeamSeasonInfo>>> GetTeamAverages(string? sort, int? page)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            var data = await repo.RetreiveNFLTeamAveragesAsync(sort, page.Value);
            if (data == null || !data.Any()) return NotFound();
            return data;
        }

        [HttpGet("teams/avg/{id}", Name = nameof(GetTeamAverage))]
        [ProducesResponseType(200, Type = typeof(NFLTeamSeasonInfo))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NFLTeamSeasonInfo>> GetTeamAverage(int id)
        {
            var data = await repo.RetreiveNFLTeamAverageAsync(id);
            if (data == null) return NotFound();
            return data;
        }

        [HttpGet("teams", Name = nameof(GetTeams))]
        [ProducesResponseType(200, Type = typeof(List<NFLTeamSeasonInfo>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<NFLTeamSeasonInfo>>> GetTeams(string? sort, int? page)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            var data = await repo.RetreiveAllNFLTeamInfoAsync(sort, page.Value);
            if (data == null || !data.Any()) return NotFound();
            return data;
        }

        [HttpGet("teams/season/{season}", Name = nameof(GetTeamsBySeason))]
        [ProducesResponseType(200, Type = typeof(List<NFLTeamSeasonInfo>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<NFLTeamSeasonInfo>>> GetTeamsBySeason(string? sort, int season, int? page)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            var data = await repo.RetreiveNFLTeamInfoBySeasonAsync(sort, season, page.Value);
            if (data == null || !data.Any()) return NotFound();
            return data;
        }

        [HttpGet("teams/{id}", Name = nameof(GetTeamByID))]
        [ProducesResponseType(200, Type = typeof(List<NFLTeamSeasonInfo>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<NFLTeamSeasonInfo>>> GetTeamByID(string? sort, int id, int? page)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            var data = await repo.RetreiveNFLTeamInfoByIDAsync(sort, id, page.Value);
            if (data == null || !data.Any()) return NotFound();
            return data;
        }

        [HttpGet("teams/{id}/{season}", Name = nameof(GetTeamBySeason))]
        [ProducesResponseType(200, Type = typeof(NFLTeamSeasonInfo))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NFLTeamSeasonInfo>> GetTeamBySeason(int id, int season)
        {
            var data = await repo.RetreiveNFLTeamSeasonInfoAsync(id, season);
            if (data == null) return NotFound();
            return data;
        }
        [HttpGet("teams/count", Name = nameof(GetTeamCount))]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> GetTeamCount(int? season)
        {
            return await repo.RetreiveNFLTeamInfoCountAsync(season);
        }


        //player actions
        [HttpGet("players", Name = nameof(GetPlayers))]
        [ProducesResponseType(200, Type = typeof(List<NFLPlayer>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<NFLPlayer>>> GetPlayers(string? sort, bool? isActive, int? page)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            if (isActive != null)
            {
                var data = await repo.RetreiveNFLPlayerInfoByStatusAsync(sort, isActive.Value, page.Value);
                if (data == null || !data.Any()) return NotFound();
                return data;
            }
            else
            {
                var data = await repo.RetreiveAllPlayerInfoAsync(sort, page.Value);
                if (data == null || !data.Any()) return NotFound();
                return data;
            }
        }

        [HttpGet("players/{id}", Name = nameof(GetPlayer))]
        [ProducesResponseType(200, Type = typeof(NFLPlayer))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NFLPlayer>> GetPlayer(int id)
        {
            var data = await repo.RetreiveNFLPlayerInfoByIDAsync(id);
            if (data == null) return NotFound();
            return data;
        }

        [HttpGet("players/count", Name = nameof(GetPlayerCount))]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> GetPlayerCount(int? teamID, bool? isActive)
        {
            return await repo.RetreiveNFLPlayerCountAsync(teamID, isActive);
        }

        [HttpGet("players/team/{id}", Name = nameof(GetPlayersByTeam))]
        [ProducesResponseType(200, Type = typeof(List<NFLPlayer>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<NFLPlayer>>> GetPlayersByTeam(string? sort, int id, int? page)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            var data = await repo.RetreiveNFLPlayerInfoByTeamAsync(sort, id, page.Value);
            if (data == null || !data.Any()) return NotFound();
            return data;
        }

        [HttpGet("SeasonMax", Name = nameof(GetMaxSeason))]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> GetMaxSeason()
        {
            return await repo.RetreiveMaxSeasonAsync();
        }

        [HttpGet("SeasonMin", Name = nameof(GetMinSeason))]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> GetMinSeason()
        {
            return await repo.RetreiveMinSeasonAsync();
        }


    }
}
