using IQSport.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IQSport.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Guidance()
    {
        var model = new SportsGuidanceViewModel
        {
            RecentHighlights = new List<Highlight>
        {
            new Highlight
            {
                Sport = "Formula 1",
                Title = "Monaco Grand Prix 2025",
                Description = "Charles Leclerc wins his home race in a thrilling finish!",
                Link = "/f1/races?raceID=123"
            },
            new Highlight
            {
                Sport = "Premier League",
                Title = "Man City vs. Liverpool",
                Description = "Man City secures a 3-2 victory in a thrilling match!",
                Link = "/premiermatch/match?searchTeam=Man%20City"
            }
        },
            QuickStats = new List<Stat>
        {
            new Stat
            {
                Sport = "Formula 1",
                Label = "Top Driver",
                Value = "Max Verstappen - 145 Points",
                Link = "/f1/alldrivers?driverID=789"
            },
            new Stat
            {
                Sport = "Premier League",
                Label = "Top Team",
                Value = "Arsenal - 65 Points",
                Link = "/premiermatch/match?searchTeam=Arsenal"
            }
        }
        };

        return View(model);
    }
}
