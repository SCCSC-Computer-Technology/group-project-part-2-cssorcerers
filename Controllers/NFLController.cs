using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportIQ.Models;
using SportsData;
using SportsData.Models;
using SportsData.Services;

namespace SportIQ.Controllers
{
    public class NFLController : Controller
    {
        private readonly ILogger<NFLController> _logger;
        private SportsDbContext _context;

        public NFLController(ILogger<NFLController> logger, SportsDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Teams()
        {
            string? pageStr = Request.Query["page"];
            string? seasonStr = Request.Query["season"];
            string? sortOrderStr = Request.Query["sort"];

            int itemsPerPage = 5;
            int.TryParse(pageStr, out int page);
            if (!int.TryParse(seasonStr, out int season))
            {
                season = -1;
            }
            else if (season != -1)
            {
                if (season > _context.NFLTeamSeasonStat.Select(x => x.Season).Max())
                {
                    season = _context.NFLTeamSeasonStat.Select(x => x.Season).Max();
                }
                else if (season < _context.NFLTeamSeasonStat.Select(x => x.Season).Min())
                {
                    season = _context.NFLTeamSeasonStat.Select(x => x.Season).Min();
                }
            }


            if (page < 1) page = 1;
            List<NFLTeamSeasonStat> teamSeasonStats;
            if (season == -1)
            {
                teamSeasonStats = await _context.NFLTeamSeasonStat.OrderBy(x => x.ID).ThenBy(x => x.Season).ToListAsync();
            }
            else
            {
                teamSeasonStats = await _context.NFLTeamSeasonStat.Distinct().Where(x => x.Season == season).OrderBy(x => x.ID).ToListAsync();
            }

            int totalItems = teamSeasonStats.Count();
            page = Math.Min(page, (int)Math.Ceiling((double)totalItems / itemsPerPage));
            int itemsToSkip = (page - 1) * itemsPerPage;

            

            NFLTeamsStatsViewModel viewModel = new NFLTeamsStatsViewModel
            {
                TeamsStats = await NFLServices.GetNFLTeamSeasonInfo(season, sortOrderStr,
                                        page, itemsPerPage, teamSeasonStats, _context),
                CurrentPage = page,
                MaxPage = (int)Math.Ceiling((double)totalItems / itemsPerPage),
                Season = season,
                SortOrder = sortOrderStr
            };
            //var teams = await _context.NFLTeam.Skip(itemsToSkip).Take(itemsPerPage).ToListAsync();
            return View(viewModel);
        }
    }
}
