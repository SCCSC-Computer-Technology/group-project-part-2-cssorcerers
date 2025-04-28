using SportsData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IQSport.ViewModels.F1;

namespace SportIQ.Controllers
{
    public class F1Controller : Controller
    {
        private readonly ILogger<F1Controller> _logger;
        private HttpClient _httpClient;
        public F1Controller(ILogger<F1Controller> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Drivers(int driverID)
        {
            try
            {
                var driverRes = await _httpClient.GetAsync($"https://localhost:5003/api/f1/drivers/{driverID}");
                if (!driverRes.IsSuccessStatusCode)
                {
                    throw new Exception($"Error retrieving driver: {driverID}");
                }
                F1DriverInfo info = await driverRes.Content.ReadFromJsonAsync<F1DriverInfo>();
                return View(info);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }


        }
        public async Task<IActionResult> AllDrivers(int? page, string? sort)
        {

            try
            {
                if (!page.HasValue || page < 1) page = 1;

                var countRes = await _httpClient.GetAsync("https://localhost:5003/api/f1/drivers/count");

                if (!countRes.IsSuccessStatusCode)
                {
                    throw new Exception("Unable to retrieve driver count");
                }

                int totalItems = await countRes.Content.ReadFromJsonAsync<int>();


                page = Math.Min(page.Value, (int)Math.Ceiling((double)totalItems / 5));


                var driverRes = await _httpClient.GetAsync($"https://localhost:5003/api/f1/drivers?sort={sort}&page={page}");

                if (!driverRes.IsSuccessStatusCode)
                {
                    throw new Exception("Unable to retrieve drivers");
                }

                List<F1DriverInfo> f1Drivers = await driverRes.Content.ReadFromJsonAsync<List<F1DriverInfo>>();

                F1DriversViewModel model = new F1DriversViewModel()
                {
                    F1Driver = f1Drivers,
                    CurrentPage = page.Value,
                    MaxPage = (int)Math.Ceiling((double)totalItems / 5)
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }
        }

        public async Task<IActionResult> AllConstructors(int? page, string? sort)
        {
            try
            {
                if (!page.HasValue || page < 1) page = 1;

                var countRes = await _httpClient.GetAsync("https://localhost:5003/api/f1/constructors/count");

                if (!countRes.IsSuccessStatusCode)
                {
                    throw new Exception("Unable to retrieve constructor count");
                }

                int totalItems = await countRes.Content.ReadFromJsonAsync<int>();
                page = Math.Min(page.Value, (int)Math.Ceiling((double)totalItems / 5));


                var constructorsRes = await _httpClient.GetAsync($"https://localhost:5003/api/f1/constructors?page={page}&sort={sort}");

                if (!constructorsRes.IsSuccessStatusCode)
                {
                    throw new Exception("Unable to retrieve constructors");
                }
                var constructors = await constructorsRes.Content.ReadFromJsonAsync<List<F1Constructor>>();



                var model = new F1ConstructorsViewModel
                {
                    F1Constructor = constructors,
                    CurrentPage = page.Value,
                    MaxPage = (int)Math.Ceiling((double)totalItems / 5),
                    SortOrder = sort
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }
        }

        public async Task<IActionResult> Constructor(int constructorID)
        {
            try
            {
                var constructoRes = await _httpClient.GetAsync($"https://localhost:5003/api/f1/constructors/{constructorID}");

                if (!constructoRes.IsSuccessStatusCode)
                {
                    throw new Exception($"Unable to retrieve constructor: {constructorID}");
                }

                var constructor = await constructoRes.Content.ReadFromJsonAsync<F1Constructor>();


                return View(constructor); // Return the Constructor.cshtml view
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }
        }

        public async Task<IActionResult> AllCurrentConstructors(int? page)
        {
            try
            {
                if (!page.HasValue || page < 1) page = 1;


                var countRes = await _httpClient.GetAsync("https://localhost:5003/api/f1/currentconstructors/count");

                if (!countRes.IsSuccessStatusCode)
                {
                    throw new Exception("Unable to retrieve current constructors count");
                }

                int totalItems = await countRes.Content.ReadFromJsonAsync<int>();
                page = Math.Min(page.Value, (int)Math.Ceiling((double)totalItems / 5));

                var constructorRes = await _httpClient.GetAsync($"https://localhost:5003/api/f1/currentconstructors?page={page}");

                if (!constructorRes.IsSuccessStatusCode)
                {
                    throw new Exception("Unable to retrieve current constructors");
                }

                var constructors = await constructorRes.Content.ReadFromJsonAsync<List<F1CurrentConstructor>>();

                var viewModel = new F1CurrentConstructorsViewModel
                {
                    F1CurrentConstructor = constructors,
                    CurrentPage = page.Value,
                    MaxPage = (int)Math.Ceiling((double)totalItems / 5)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }
        }

        public async Task<IActionResult> CurrentConstructors(int? constructorID)
        {
            try
            {
                if (!constructorID.HasValue)
                {
                    return Redirect("/home/error");
                }


                var constructorRes = await _httpClient.GetAsync($"https://localhost:5003/api/f1/constructors/{constructorID.Value}");

                if (!constructorRes.IsSuccessStatusCode)
                {
                    throw new Exception($"Unable to retrieve constructor {constructorID}");
                }

                F1CurrentConstructor team = await constructorRes.Content.ReadFromJsonAsync<F1CurrentConstructor>();
                return View(team);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }
        }

        public async Task<IActionResult> AllRaces(int? page)
        {
            try
            {
                if (!page.HasValue || page < 1) page = 1;

                var countRes = await _httpClient.GetAsync("https://localhost:5003/api/f1/races/count");

                if (!countRes.IsSuccessStatusCode)
                {
                    throw new Exception("Unable to retrieve race count");
                }

                int totalItems = await countRes.Content.ReadFromJsonAsync<int>();


                page = Math.Min(page.Value, (int)Math.Ceiling((double)totalItems / 10));




                var racesRes = await _httpClient.GetAsync($"https://localhost:5003/api/f1/races?page={page}");

                if (!racesRes.IsSuccessStatusCode)
                {
                    throw new Exception("Error retrieving races");
                }

                var races = await racesRes.Content.ReadFromJsonAsync<List<F1RaceInfo>>();


                var model = new F1RacesViewModel
                {
                    F1Races = races,
                    CurrentPage = page.Value,
                    MaxPage = (int)Math.Ceiling((double)totalItems / 10)
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }
        }

        public async Task<IActionResult> Races(int? raceID)
        {
            try
            {
                if (!raceID.HasValue)
                {
                    return Redirect("/home/error");
                }

                var raceRes = await _httpClient.GetAsync($"https://localhost:5003/api/f1/races/{raceID}");

                if (!raceRes.IsSuccessStatusCode)
                {
                    throw new Exception("Unable to retrieve race");
                }
            

                var info = await raceRes.Content.ReadFromJsonAsync<F1RaceInfo>();
                return View(info);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Race] {DateTime.Now}: {ex.Message}");
                return Redirect("/home/error");
            }
        }





    }
}
