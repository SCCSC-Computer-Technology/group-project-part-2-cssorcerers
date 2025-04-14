using IQSport.Data.DbContext;
using IQSport.Models.SportsModels.F1.Models;
using IQSport.Models.SportsModels.F1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsData.Services;

namespace SportIQ.Controllers
{
    public class F1Controller : Controller
    {
        private readonly ILogger<F1Controller> _logger;
        private SportsDbContext _context;

        public F1Controller(ILogger<F1Controller> logger, SportsDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Drivers()
        {
            string? driverIDStr = Request.Query["driverID"];

            if (!int.TryParse(driverIDStr, out int driverID))
            {
                return Redirect("/home/error");
            }

            try
            {
                F1DriverInfo info = await F1Services.GetF1DriverInfo(driverID, _context);
                return View(info);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }


        }
        public async Task<IActionResult> AllDrivers()
        {
            string? pageStr = Request.Query["page"];
            string? driverIDStr = Request.Query["driverID"];

            int itemsPerPage = 5;
            int.TryParse(pageStr, out int page);
            if (page < 1) page = 1;

            int driverID = -1;
            if (!string.IsNullOrEmpty(driverIDStr))
            {
                if (!int.TryParse(driverIDStr, out driverID))
                {
                    return Redirect("/home/error");  // If parsing fails, redirect to error
                }
            }


            //List<F1DriverInfo> f1Drivers;


            var query = _context.F1Driver.AsQueryable();

            if (driverID != -1)
            {
                query = query.Where(x => x.DriverID == driverID);  // Filter by driverID if specified
            }

            int totalItems = query.Count();

            var f1Drivers = await query
               .Skip((page - 1) * itemsPerPage)  // Skipping the appropriate number of items based on the page
               .Take(itemsPerPage)  // Taking the items for the current page
               .Select(driver => new F1DriverInfo
               {
                   DriverID = driver.DriverID,
                   DriverRef = driver.DriverRef,
                   Number = driver.Number,
                   Code = driver.Code,
                   Name = driver.Name,
                   Dob = driver.Dob,  // Only the date part
                   Nationality = driver.Nationality,
                   Url = driver.Url
               })
               .ToListAsync();


            page = Math.Min(page, (int)Math.Ceiling((double)totalItems / itemsPerPage));


            F1DriversViewModel model = new F1DriversViewModel()
            {
                F1Driver = f1Drivers,
                CurrentPage = page,
                MaxPage = (int)Math.Ceiling((double)totalItems / itemsPerPage),
                DriverID = driverID
            };

            return View(model);
        }

        public async Task<IActionResult> AllConstructors()
        {
            string? pageStr = Request.Query["page"];
            string? sortOrder = Request.Query["sort"];

            int itemsPerPage = 5;
            int.TryParse(pageStr, out int page);
            if (page < 1) page = 1;

            var query = _context.F1Constructor.AsQueryable();

            // Optional sorting logic
            sortOrder = sortOrder?.ToLower();
            switch (sortOrder)
            {
                case "name_asc":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                default:
                    query = query.OrderBy(x => x.ConstructorID); // Default sort
                    break;
            }

            int totalItems = await query.CountAsync();

            var constructors = await query
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(constructor => new F1ConstructorInfo
                {
                    ConstructorID = constructor.ConstructorID,
                    ConstructorRef = constructor.ConstructorRef,
                    Name = constructor.Name,
                    Nationality = constructor.Nationality,
                    Url = constructor.Url
                })
                .ToListAsync();

            page = Math.Min(page, (int)Math.Ceiling((double)totalItems / itemsPerPage));

            var model = new F1ConstructorsViewModel
            {
                F1Constructor = constructors,
                CurrentPage = page,
                MaxPage = (int)Math.Ceiling((double)totalItems / itemsPerPage),
                SortOrder = sortOrder
            };

            return View(model);
        }

        public async Task<IActionResult> Constructor(int constructorID)
        {
            try
            {
                var constructor = await _context.F1Constructor
                    .Where(c => c.ConstructorID == constructorID)
                    .Select(c => new F1ConstructorInfo
                    {
                        ConstructorID = c.ConstructorID,
                        ConstructorRef = c.ConstructorRef,
                        Name = c.Name,
                        Nationality = c.Nationality,
                        Url = c.Url
                    })
                    .FirstOrDefaultAsync();

                if (constructor == null)
                {
                    return Redirect("/home/error");
                }

                return View(constructor); // Return the Constructor.cshtml view
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }
        }

        public async Task<IActionResult> AllCurrentConstructors()
        {
            string? pageStr = Request.Query["page"];
            int.TryParse(pageStr, out int page);
            if (page < 1) page = 1;

            int itemsPerPage = 5;

            var query = _context.F1CurrentConstructor.AsQueryable();

            int totalItems = await query.CountAsync();

            var constructors = await query
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(x => new F1CurrentConstructorInfo
                {
                    ConstructorID = x.ConstructorID,
                    TeamName = x.TeamName,
                    Principal = x.Principal,
                    Driver1 = x.Driver1,
                    D1Num = x.D1Num,
                    Driver2 = x.Driver2,
                    D2Num = x.D2Num,
                    Championships = x.Championships,
                    Base = x.Base,
                    PowerUnit = x.PowerUnit
                })
                .ToListAsync();

            page = Math.Min(page, (int)Math.Ceiling((double)totalItems / itemsPerPage));

            var viewModel = new F1CurrentConstructorsViewModel
            {
                F1CurrentConstructor = constructors,
                CurrentPage = page,
                MaxPage = (int)Math.Ceiling((double)totalItems / itemsPerPage)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> CurrentConstructors()
        {
            string? constructorIDStr = Request.Query["constructorID"];

            if (!int.TryParse(constructorIDStr, out int constructorID))
            {
                return Redirect("/home/error");
            }

            try
            {
                F1CurrentConstructorInfo team = await F1Services.GetF1CurrentConstructorInfo(constructorID, _context);
                return View(team);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}:\n\t{ex.Message}");
                return Redirect("/home/error");
            }
        }

        public async Task<IActionResult> AllRaces()
        {
            string? pageStr = Request.Query["page"];
            string? yearStr = Request.Query["year"];
            string? roundStr = Request.Query["round"];

            int.TryParse(pageStr, out int page);
            int.TryParse(yearStr, out int year);
            int.TryParse(roundStr, out int round);

            if (page < 1) page = 1;
            int itemsPerPage = 10;

            var query = _context.F1Race.AsQueryable();

            int totalItems = await query.CountAsync();

            var races = await query
                .OrderByDescending(r => r.Year).ThenBy(r => r.Round)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Join(_context.F1Circuit,
                      race => race.CircuitID,
                      circuit => circuit.CircuitID,
                      (race, circuit) => new F1RaceInfo
                      {
                          RaceID = race.RaceID,
                          Name = race.Name,
                          Year = race.Year,
                          Round = race.Round,
                          Date = race.Date,
                          Time = race.Time,
                          CircuitName = circuit.Name, // ← this is what you want
                          Url = race.Url
                      })
                .ToListAsync();

            var model = new F1RacesViewModel
            {
                F1Races = races,
                CurrentPage = page,
                MaxPage = (int)Math.Ceiling((double)totalItems / itemsPerPage)
            };

            return View(model);
        }

        public async Task<IActionResult> Races()
        {
            string? raceIDStr = Request.Query["raceID"];

            if (!int.TryParse(raceIDStr, out int raceID))
                return Redirect("/home/error");

            try
            {
                var info = await F1Services.GetF1RaceInfo(raceID, _context);
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
