using SportsData.Models;

using IQSport.Models.SportsModels.CSGO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportIQ.Controllers;
using IQSport.ViewModels.Premier;

namespace IQSport.Controllers
{
    public class CSGOController : Controller
    {
        private readonly ILogger<CSGOController> _logger;
        private HttpClient _httpClient;

        public CSGOController(ILogger<CSGOController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        [Authorize]
        public async Task<IActionResult> CSGOView(PlayerFilterViewModel filter)
        {
            try
            {
                int page = filter.CurrentPage < 1 ? 1 : filter.CurrentPage;
                filter.PageSize = filter.PageSize == 0 ? 10 : filter.PageSize;

                var countRes = await _httpClient.GetAsync("https://localhost:5003/api/csgo/players/count");

                if (!countRes.IsSuccessStatusCode)
                {
                    throw new Exception("Unable to retrieve player count");
                }

                int totalItems = await countRes.Content.ReadFromJsonAsync<int>();

                int maxPage = totalItems / filter.PageSize;

                page = Math.Min(page, maxPage);

                var csgoRes = await _httpClient.GetAsync("https://localhost:5003/api/csgo/players?" +
                    $"name={filter.SearchTerm}&minRating={filter.MinRating}&minmaps={filter.MinMaps}" +
                    $"&sort={filter.SortBy}&isDesc={filter.SortDescending}&page={filter.CurrentPage}&pagesize={filter.PageSize}");

                if (!csgoRes.IsSuccessStatusCode)
                {
                    return View(filter);
                }

                var players = await csgoRes.Content.ReadFromJsonAsync<List<CSGOPlayer>>();

                var pagination = new PaginationViewModel()
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize),
                    PageSize = filter.PageSize,
                    TotalItems = totalItems
                };

                ViewBag.Players = players;
                ViewBag.Pagination = pagination; // Pass pagination data to the view
                return View(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }
        }
    }
}