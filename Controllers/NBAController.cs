using IQSport.Data.DbContext;
using IQSport.Models;
using IQSport.Models.SportsModels.NBA.Models;
using IQSport.Models.SportsModels.NBA.ViewModels;
using IQSports.Models.SportModels.NBA.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsData.Services;

namespace SportIQ.Controllers
{
    public class NBAController : Controller
    {
        private readonly ILogger<NBAController> _logger;
        private SportsDbContext _context;

        public NBAController(ILogger<NBAController> logger, SportsDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
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
                if (season > _context.NBATeamSeasonStat.Select(x => x.Season).Max())
                {
                    season = _context.NBATeamSeasonStat.Select(x => x.Season).Max();
                }
                else if (season < _context.NBATeamSeasonStat.Select(x => x.Season).Min())
                {
                    season = _context.NBATeamSeasonStat.Select(x => x.Season).Min();
                }
            }


            if (page < 1) page = 1;
            List<NBATeamSeasonStat> teamSeasonStats;
            if (season == -1)
            {
                teamSeasonStats = await _context.NBATeamSeasonStat.OrderBy(x => x.TeamID).ThenBy(x => x.Season).ToListAsync();
            }
            else
            {
                teamSeasonStats = await _context.NBATeamSeasonStat.Distinct().Where(x => x.Season == season).OrderBy(x => x.TeamID).ToListAsync();
            }

            int totalItems = teamSeasonStats.Count();
            page = Math.Min(page, (int)Math.Ceiling((double)totalItems / itemsPerPage));


            try
            {
                if (sortOrderStr == null) sortOrderStr = "";
                NBATeamsStatsViewModel viewModel = new NBATeamsStatsViewModel
                {

                    TeamsStats = await NBAServices.GetNBATeamSeasonInfoList(season, sortOrderStr,
                                            page, itemsPerPage, teamSeasonStats, _context),
                    CurrentPage = page,
                    MaxPage = (int)Math.Ceiling((double)totalItems / itemsPerPage),
                    Season = season,
                    SortOrder = sortOrderStr
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }
        }

        public async Task<IActionResult> Players()
        {
            string? pageStr = Request.Query["page"];
            string? sortOrderStr = Request.Query["sort"];
            string? teamIDStr = Request.Query["team"];
            string? isActiveStr = Request.Query["active"];

            int itemsPerPage = 5;
            int.TryParse(pageStr, out int page);

            if (!int.TryParse(teamIDStr, out int teamID))
            {
                teamID = -1;
            }

            bool? isActive = null;
            try
            {
                isActive = bool.Parse(isActiveStr);
            }
            catch { }


            if (page < 1) page = 1;



            List<NBAPlayerInfo> nbaPlayers;
            try
            {
                if (sortOrderStr == null) sortOrderStr = "";
                nbaPlayers = await NBAServices.GetNBAPlayerInfoList(isActive, teamID, sortOrderStr, page, itemsPerPage, _context);
            }
            catch
            {
                return Redirect("/home/error");
            }
            int totalItems = _context.NBAPlayer.Where(x => (teamID == -1 || x.TeamID == teamID) &&
            (!isActive.HasValue || (isActive.HasValue && x.IsActive == isActive))).Count();
            page = Math.Min(page, (int)Math.Ceiling((double)totalItems / itemsPerPage));


            NBAPlayersStatsViewModel model = new()
            {
                nbaPlayers = nbaPlayers,
                CurrentPage = page,
                MaxPage = (int)Math.Ceiling((double)totalItems / itemsPerPage),
                TeamID = teamID,
                SortOrder = sortOrderStr,
                IsActive = isActive
            };


            return View(model);
        }


    }
}
