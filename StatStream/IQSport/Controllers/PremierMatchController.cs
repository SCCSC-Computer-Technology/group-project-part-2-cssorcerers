
using IQSport.ViewModels.Premier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsData.Models;

namespace IQSport.Controllers
{
    public class PremierMatchController : Controller
    {
        private readonly ILogger<PremierMatchController> _logger;
        private HttpClient _httpClient;
        private const int PageSize = 10;
        private const int CacheDurationMinutes = 15;

        public PremierMatchController(ILogger<PremierMatchController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
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
                var response = await _httpClient.GetAsync("https://localhost:5003/api/premier/matches?" +
                    $"sort={sortOrder}&page={page}&itemsPerPage={PageSize}&searchTerm={searchTeam}&dataFrom={dateFrom}&dateTo={dateTo}");
                List<PremierMatch> matchList = new List<PremierMatch>();
                if (response.IsSuccessStatusCode)
                {
                    matchList = await response.Content.ReadFromJsonAsync<List<PremierMatch>>();
                }

                var matchesQuery = matchList.Select(m => new MatchViewModel()
                {
                    Match = m,
                    ScoreDisplay = $"{m.FullTimeHomeGoals} - {m.FullTimeAwayGoals}",
                    HalfTimeScoreDisplay = m.HalfTimeHomeGoals.HasValue && m.HalfTimeAwayGoals.HasValue
                                            ? $"{m.HalfTimeHomeGoals} - {m.HalfTimeAwayGoals}" : "N/A"
                });

                var countResponse = await _httpClient.GetAsync("https://localhost:5003/api/premier/matches/count?" +
                     $"searchTerm={searchTeam}&dataFrom={dateFrom}&dateTo={dateTo}");

                if (!countResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Could not retrieve premier match count");
                    throw new Exception();
                }
                ;

                int totalCount = await countResponse.Content.ReadFromJsonAsync<int>();
                // ViewModel for pagination and filters
                var viewModel = new PremierMatchesViewModel
                {
                    Matches = matchesQuery.ToList(),
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