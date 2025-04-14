using IQSport.Data.Services;
using IQSport.Models.SportsModels.CSGO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IQSport.Controllers
{
    public class CSGOController : Controller
    {
        private readonly ICSGOService _csgoService;

        public CSGOController(ICSGOService csgoService)
        {
            _csgoService = csgoService;
        }

        [Authorize]
        public async Task<IActionResult> CSGOView(PlayerFilterViewModel filter)
        {
            var (players, pagination) = await _csgoService.GetPlayers(filter);
            ViewBag.Players = players;
            ViewBag.Pagination = pagination; // Pass pagination data to the view
            return View(filter);
        }
    }
}