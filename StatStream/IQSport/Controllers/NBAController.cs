using SportsData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IQSport.ViewModels.NBA;

namespace SportIQ.Controllers
{
    public class NBAController : Controller
    {
        private readonly ILogger<NBAController> _logger;
        private HttpClient _httpClient;
        private int maxYear;
        private int minYear;

        public NBAController(ILogger<NBAController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            minYear = 1950;
            maxYear = 2024;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Teams(int? page, int? season, string? sort)
        {
            try
            {
                if (!season.HasValue) season = -1;
                if (season != -1)
                {
                    var maxRes = await _httpClient.GetAsync("https://localhost:5003/api/nba/seasonmax");
                    if (maxRes.IsSuccessStatusCode)
                    {
                        maxYear = await maxRes.Content.ReadFromJsonAsync<int>();
                    }

                    var minRes = await _httpClient.GetAsync("https://localhost:5003/api/nba/seasonmin");
                    if (minRes.IsSuccessStatusCode)
                    {
                        minYear = await minRes.Content.ReadFromJsonAsync<int>();
                    }

                    if (season > maxYear)
                    {
                        season = maxYear;
                    }
                    else if (season < minYear)
                    {
                        season = minYear;
                    }
                }


                if (!page.HasValue || page < 1) page = 1;

                var countRes = await _httpClient.GetAsync($"https://localhost:5003/api/nba/teams/count?season={season}");
                if (!countRes.IsSuccessStatusCode)
                {
                    throw new Exception("Could not retrieve a count of teams");
                }



                int totalItems = await countRes.Content.ReadFromJsonAsync<int>();
                page = Math.Min(page.Value, (int)Math.Ceiling((double)totalItems / 5));

                HttpResponseMessage teamsRes;
                if (season != -1)
                {
                    teamsRes = await _httpClient.GetAsync($"https://localhost:5003/api/nba/teams/season/{season}?sort={sort}&page={page}");
                }
                else
                {
                    teamsRes = await _httpClient.GetAsync($"https://localhost:5003/api/nba/teams?sort={sort}&page={page}");
                }
                if (!teamsRes.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to find teams");
                }

                List<NBATeamSeasonInfo> stats = await teamsRes.Content.ReadFromJsonAsync<List<NBATeamSeasonInfo>>();


                NBATeamsStatsViewModel viewModel = new NBATeamsStatsViewModel
                {

                    TeamsStats = stats,
                    CurrentPage = page.Value,
                    MaxPage = (int)Math.Ceiling((double)totalItems / 5),
                    Season = season.Value,
                    SortOrder = sort
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }
        }

        public async Task<IActionResult> Players(int? page, int? team, bool? active, string? sort)
        {
            try
            {
                if (!team.HasValue)
                {
                    team = -1;
                }
                if (!page.HasValue || page < 1) page = 1;



                var countRes = await _httpClient.GetAsync($"https://localhost:5003/api/nba/players/count?isActive={active}&teamID={team}");
                if (!countRes.IsSuccessStatusCode)
                {
                    throw new Exception("Unable to get count for NBA players");
                }
                int totalItems = await countRes.Content.ReadFromJsonAsync<int>();

                page = Math.Min(page.Value, (int)Math.Ceiling((double)totalItems / 5));

                HttpResponseMessage playersRes;

                if (team == -1)
                {
                    playersRes = await _httpClient.GetAsync($"https://localhost:5003/api/nba/players/team/{team}?sort={sort}&page={page}");
                }
                else
                {
                    playersRes = await _httpClient.GetAsync($"https://localhost:5003/api/nba/players?sort={sort}&page={page}&isActive={active}");
                }
                if (!playersRes.IsSuccessStatusCode)
                {
                    throw new Exception("Unable to get NBA players");
                }


                List<NBAPlayer> nbaPlayers = await playersRes.Content.ReadFromJsonAsync<List<NBAPlayer>>();




                NBAPlayersStatsViewModel model = new()
                {
                    nbaPlayers = nbaPlayers,
                    CurrentPage = page.Value,
                    MaxPage = (int)Math.Ceiling((double)totalItems / 5),
                    TeamID = team.Value,
                    SortOrder = sort,
                    IsActive = active
                };


                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return View("/home/error");
            }
        }


        public async Task<IActionResult> Team(int? id, int? season)
        {
            string? teamIDStr = Request.Query["id"];
            string? seasonStr = Request.Query["season"];

            if (!id.HasValue)
            {
                id = 1;
            }
            if (!season.HasValue)
            {
                season = -1;
            }

            try
            {
                HttpResponseMessage teamRes;
                if(season.Value == -1)
                {
                    teamRes = await _httpClient.GetAsync($"https://localhost:5003/api/nba/teams/avg/{id}");
                }
                else
                {
                    teamRes = await _httpClient.GetAsync($"https://localhost:5003/api/nba/teams/{id}/{season}");
                }

                if (!teamRes.IsSuccessStatusCode)
                {
                    throw new Exception("Error retrieving team info");
                }

                NBATeamSeasonInfo info = await teamRes.Content.ReadFromJsonAsync<NBATeamSeasonInfo>();
                return View(info);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }


        }

        public async Task<IActionResult> Player(int id)
        {

            try
            {
                var playerRes = await _httpClient.GetAsync($"https://localhost:5003/api/nba/player/{id}");
                if (!playerRes.IsSuccessStatusCode)
                {
                    throw new Exception($"Unable to find a player with the ID: {id}");
                }
                NBAPlayer player = await playerRes.Content.ReadFromJsonAsync<NBAPlayer>();
                return View(player);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }


        }
    }
}
