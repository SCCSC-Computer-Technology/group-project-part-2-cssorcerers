using Azure;
using Microsoft.AspNetCore.Mvc;
using SportsAPI.Interfaces;
using SportsData.Models;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SportsAPI.Controllers
{
    [Route("api/premier")]
    [ApiController]
    public class PremierApiController : ControllerBase
    {
        private readonly IPremierRepository repo;

        public PremierApiController(IPremierRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet("matches", Name = nameof(GetAllPremierMatches))]
        [ProducesResponseType(200, Type = typeof(List<PremierMatch>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<PremierMatch>>> GetAllPremierMatches(string? sort, int? page, int? itemsPerPage,
            string? searchTerm, DateTime? dateFrom, DateTime? dateTo)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            if(!itemsPerPage.HasValue || itemsPerPage.Value < 1) itemsPerPage = 5;
            var data = await repo.RetrievePremierMatchesAsync(sort, page.Value, itemsPerPage.Value, searchTerm, dateFrom, dateTo);

            if (data == null || !data.Any()) return NotFound();
            return data;

        }

        [HttpGet("matches/count", Name = nameof(GetPremierMatchesCount))]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<int>> GetPremierMatchesCount(string? searchTerm, DateTime? dateFrom, DateTime? dateTo)
        {
            var data = await repo.RetrievePremierMatchesCountAsync(searchTerm, dateFrom, dateTo);

            if (data == null) return NotFound();
            return data;

        }

        [HttpGet("matches/byteam/{teamName}", Name = nameof(GetPremierMatchesByTeam))]
        [ProducesResponseType(200, Type = typeof(List<PremierMatch>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<PremierMatch>>> GetPremierMatchesByTeam(string? sort, string? teamName, int? page)
        {
            if (!page.HasValue || page.Value < 1) page = 1;
            var data = await repo.RetrievePremierMatchesByTeamNameAsync(sort, teamName, page ?? 1);

            if (data == null || !data.Any()) return NotFound();
            return data;
        }

        [HttpGet("date/max", Name = nameof(GetMaxDate))]
        [ProducesResponseType(200, Type = typeof(DateTime))]
        public async Task<ActionResult<DateTime>> GetMaxDate()
        {
            return await repo.RetrieveMaxDateAsync();
        }

        [HttpGet("date/min", Name = nameof(GetMinDate))]
        [ProducesResponseType(200, Type = typeof(DateTime))]
        public async Task<ActionResult<DateTime>> GetMinDate()
        {
            return await repo.RetrieveMinDateAsync();
        }

    }
}
