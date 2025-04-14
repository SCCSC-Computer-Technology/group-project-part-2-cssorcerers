using IQSport.Data.DbContext;
using IQSport.Models.SportsModels.Premier.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IQSport.Controllers
{
    public class PremierMatchController : Controller
    {
        private readonly SportsDbContext _context;
        private readonly ILogger<PremierMatchController> _logger;
        private const int PageSize = 10;
        private const int CacheDurationMinutes = 15;

        public PremierMatchController(ILogger<PremierMatchController> logger, SportsDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        [Authorize]
        [ResponseCache(Duration = CacheDurationMinutes * 60, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Match(
            string sortOrder = "date_desc",
            int page = 1,
            string searchTeam = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null)
        {
            try
            {
                // Base query with eager loading
                var matchesQuery = _context.PremierMatch
                    .AsNoTracking()
                    .Include(m => m.HomeTeamNavigation)
                    .Include(m => m.AwayTeamNavigation)
                    .Select(match => new MatchViewModel
                    {
                        MatchID = match.MatchID,
                        Date = match.Date,
                        HomeTeamName = match.HomeTeamNavigation.TeamName,
                        AwayTeamName = match.AwayTeamNavigation.TeamName,
                        FullTimeHomeGoals = match.FullTimeHomeGoals,
                        FullTimeAwayGoals = match.FullTimeAwayGoals,
                        FullTimeResult = match.FullTimeResult,
                        HalfTimeHomeGoals = match.HalfTimeHomeGoals,
                        HalfTimeAwayGoals = match.HalfTimeAwayGoals,
                        HalfTimeResult = match.HalfTimeResult,
                        Referee = match.Referee,
                        HomeShots = match.HomeShots,
                        AwayShots = match.AwayShots,
                        HomeShotsOnTarget = match.HomeShotsOnTarget,
                        AwayShotsOnTarget = match.AwayShotsOnTarget,
                        HomeFouls = match.HomeFouls,
                        AwayFouls = match.AwayFouls,
                        HomeCorners = match.HomeCorners,
                        AwayCorners = match.AwayCorners,
                        HomeYellowCards = match.HomeYellowCards,
                        AwayYellowCards = match.AwayYellowCards,
                        HomeRedCards = match.HomeRedCards,
                        AwayRedCards = match.AwayRedCards,
                        ScoreDisplay = match.ScoreDisplay,
                        HalfTimeScoreDisplay = match.HalfTimeScoreDisplay
                    });

                // Filtering
                if (!string.IsNullOrEmpty(searchTeam))
                {
                    searchTeam = searchTeam.ToLower();
                    matchesQuery = matchesQuery.Where(m =>
                        m.HomeTeamName.ToLower().Contains(searchTeam) ||
                        m.AwayTeamName.ToLower().Contains(searchTeam));
                }

                if (dateFrom.HasValue)
                {
                    matchesQuery = matchesQuery.Where(m => m.Date >= dateFrom.Value);
                }

                if (dateTo.HasValue)
                {
                    matchesQuery = matchesQuery.Where(m => m.Date <= dateTo.Value);
                }

                // Enhanced sorting
                matchesQuery = sortOrder switch
                {
                    "date_asc" => matchesQuery.OrderBy(m => m.Date),
                    "date_desc" => matchesQuery.OrderByDescending(m => m.Date),
                    "referee_asc" => matchesQuery.OrderBy(m => m.Referee),
                    "referee_desc" => matchesQuery.OrderByDescending(m => m.Referee),
                    "hometeam_asc" => matchesQuery.OrderBy(m => m.HomeTeamName),
                    "hometeam_desc" => matchesQuery.OrderByDescending(m => m.HomeTeamName),
                    "goals_asc" => matchesQuery.OrderBy(m => m.FullTimeHomeGoals + m.FullTimeAwayGoals),
                    "goals_desc" => matchesQuery.OrderByDescending(m => m.FullTimeHomeGoals + m.FullTimeAwayGoals),
                    _ => matchesQuery.OrderByDescending(m => m.Date)
                };

                // Pagination
                var totalCount = await matchesQuery.CountAsync();
                var matches = await matchesQuery
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                // ViewModel for pagination and filters
                var viewModel = new PremierMatchesViewModel
                {
                    Matches = matches,
                    Pagination = new PaginationViewModel
                    {
                        CurrentPage = page,
                        TotalPages = (int)Math.Ceiling((double)totalCount / PageSize),
                        PageSize = PageSize,
                        TotalItems = totalCount
                    },
                    CurrentSort = sortOrder,
                    SearchTeam = searchTeam,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log exception (implement your logging)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}