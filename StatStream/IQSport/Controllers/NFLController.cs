using SportsData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IQSport.ViewModels.NFL;
using Microsoft.EntityFrameworkCore;


namespace SportIQ.Controllers
{
    public class NFLController : Controller
    {
        private readonly ILogger<NFLController> _logger;
        private HttpClient _httpClient;
        private int maxYear;
        private int minYear;
        public NFLController(ILogger<NFLController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            minYear = 1970;
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

                if(!season.HasValue)
                {
                    season = -1;
                }
                else if (season != -1)
                {
                    var maxYearRes = await _httpClient.GetAsync("https://localhost:5003/api/nfl/seasonmax");
                    if (maxYearRes.IsSuccessStatusCode)
                    {
                        maxYear = await maxYearRes.Content.ReadFromJsonAsync<int>();
                    }

                    var minYearRes = await _httpClient.GetAsync("https://localhost:5003/api/nfl/seasonmin");
                    if (maxYearRes.IsSuccessStatusCode)
                    {
                        minYear = await minYearRes.Content.ReadFromJsonAsync<int>();
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

                var countRes = await _httpClient.GetAsync($"https://localhost:5003/api/nfl/teams/count?season={season}");
                if (!countRes.IsSuccessStatusCode)
                {
                    _logger.LogError("Could not retrieve team info count");
                    throw new Exception();
                }
                int totalItems = await countRes.Content.ReadFromJsonAsync<int>();
                page = Math.Min(page.Value, (int)Math.Ceiling((double)totalItems / 5));

                List<NFLTeamSeasonInfo> teamSeasonStats = new List<NFLTeamSeasonInfo>();
                if (season == -1)
                {
                    

                    var teamRes = await _httpClient.GetAsync($"https://localhost:5003/api/nfl/teams?sort={sort}&page={page}");
                    if (teamRes.IsSuccessStatusCode)
                    {
                        teamSeasonStats = await teamRes.Content.ReadFromJsonAsync<List<NFLTeamSeasonInfo>>();
                    }
                }
                else
                {
                    var teamRes = await _httpClient.GetAsync($"https://localhost:5003/api/nfl/teams/season/{season.Value}?sort={sort}&page={page}");
                    if (teamRes.IsSuccessStatusCode)
                    {
                        teamSeasonStats = await teamRes.Content.ReadFromJsonAsync<List<NFLTeamSeasonInfo>>();
                    }
                }




                NFLTeamsStatsViewModel viewModel = new NFLTeamsStatsViewModel
                {

                    TeamsStats = teamSeasonStats,
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

        public async Task<IActionResult> Players(int? page, string? sort, int? teamID, bool? isActive)
        {
            try
            {
                if (!teamID.HasValue)
                {
                    teamID = -1;
                }


                if (!page.HasValue || page < 1) page = 1;



                var countRes = await _httpClient.GetAsync($"https://localhost:5003/api/nfl/players/count?teamID={teamID}&status={isActive}");
                if (!countRes.IsSuccessStatusCode)
                {
                    throw new Exception("Could not retrieve player info count");
                }
                int totalItems = await countRes.Content.ReadFromJsonAsync<int>();
                page = Math.Min(page.Value, (int)Math.Ceiling((double)totalItems / 5));

                HttpResponseMessage playersRes;
                if(teamID.HasValue)
                {
                    playersRes = await _httpClient.GetAsync($"https://localhost:5003/api/nfl/players/team/{teamID.Value}?page={page}");
                }
                else if(isActive.HasValue)
                {
                    playersRes = await _httpClient.GetAsync($"https://localhost:5003/api/nfl/players?isActive={isActive.Value}&page={page}");
                }
                else
                {
                    playersRes = await _httpClient.GetAsync($"https://localhost:5003/api/nfl/players?page={page}");
                }

                List<NFLPlayer> nflPlayers;
                if (playersRes.IsSuccessStatusCode)
                {
                    nflPlayers = await playersRes.Content.ReadFromJsonAsync<List<NFLPlayer>>();
                }
                else
                {
                    return Redirect("home/error");
                }


                NFLPlayersStatsViewModel model = new()
                {
                    nflPlayers = nflPlayers,
                    CurrentPage = page.Value,
                    MaxPage = (int)Math.Ceiling((double)totalItems / 5),
                    TeamID = teamID.Value,
                    SortOrder = sort,
                    IsActive = isActive
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }
        }


        public async Task<IActionResult> Team(int? id, int? season)
        {
            
            if (!id.HasValue)
            {
                id = 1;
            }

            if (!season.HasValue)
            {
                season = -1;
            }


            HttpResponseMessage res;
            try
            {
                if (season == -1)
                {
                    res = await _httpClient.GetAsync($"https://localhost:5003/api/nfl/teams/avg/{id}");

                }
                else
                {
                    res = await _httpClient.GetAsync($"https://localhost:5003/api/nfl/teams/{id}/{season}");
                }

                if (res.IsSuccessStatusCode)
                {
                    NFLTeamSeasonInfo info = await res.Content.ReadFromJsonAsync<NFLTeamSeasonInfo>();
                    return View(info);
                }
                else
                {
                    throw new Exception("Failed to get a response for team info");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }

        }

        public async Task<IActionResult> Player(int? id)
        {
            if(!id.HasValue) return Redirect("/home/error");


            try
            {
                var res = await _httpClient.GetAsync($"https://localhost:5003/api/nfl/player/{id}");

                if(res.IsSuccessStatusCode)
                {
                    NFLPlayer player = await res.Content.ReadFromJsonAsync<NFLPlayer>();
                    return View(player);
                }
                else
                {
                    throw new Exception($"Failed to find a player with the id: {id}");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }


        }
    }
}
