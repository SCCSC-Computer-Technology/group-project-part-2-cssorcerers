//using Microsoft.AspNetCore.Mvc;
//using SportsAPI.Data.Repos;

//namespace SportsAPI.Controllers
//{

//    [ApiController]
//    [Route("api/v1")]
//    public class SportsController : ControllerBase
//    {
//        private readonly ISportsDataService _dataService;

//        public SportsController(ISportsDataService dataService)
//        {
//            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
//        }

//        [HttpGet("sports")]
//        public IActionResult GetSports()
//        {
//            var sports = _dataService.GetSports();
//            return Ok(sports);
//        }

//        [HttpGet("{sport}/highlights")]
//        public async Task<IActionResult> GetHighlights(string sport, [FromQuery] int page = 1, [FromQuery] int size = 10)
//        {
//            if (string.IsNullOrWhiteSpace(sport) || !_dataService.IsValidSport(sport))
//            {
//                return BadRequest(new { Error = "Invalid or unsupported sport." });
//            }
//            if (page < 1 || size < 1)
//            {
//                return BadRequest(new { Error = "Page and size must be positive integers." });
//            }

//            try
//            {
//                var highlights = await _dataService.GetHighlightsAsync(sport, page, size);
//                return Ok(highlights);
//            }
//            catch (Exception ex)
//            {
//                // TODO: Add logging (e.g., ILogger)
//                return StatusCode(500, new { Error = "An error occurred while fetching highlights." });
//            }
//        }

//        [HttpGet("{sport}/schedule")]
//        public async Task<IActionResult> GetSchedule(string sport, [FromQuery] int page = 1, [FromQuery] int size = 10)
//        {
//            if (string.IsNullOrWhiteSpace(sport) || !_dataService.IsValidSport(sport))
//            {
//                return BadRequest(new { Error = "Invalid or unsupported sport." });
//            }
//            if (page < 1 || size < 1)
//            {
//                return BadRequest(new { Error = "Page and size must be positive integers." });
//            }

//            try
//            {
//                var schedule = await _dataService.GetScheduleAsync(sport, page, size);
//                return Ok(schedule);
//            }
//            catch (Exception ex)
//            {
//                // TODO: Add logging (e.g., ILogger)
//                return StatusCode(500, new { Error = "An error occurred while fetching schedule." });
//            }
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using SportsAPI.Data.Repos;
using SportsAPI.Models;

namespace SportsAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1")]
    public class SportsController : ControllerBase
    {
        private readonly ISportsDataService _dataService;

        public SportsController(ISportsDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [HttpGet("sports")]
        public IActionResult GetSports()
        {
            var sports = _dataService.GetSports();
            return Ok(sports);
        }

        [HttpGet("highlights")]
        public async Task<IActionResult> GetAllHighlights([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            if (page < 1 || size < 1)
            {
                return BadRequest(new { Error = "Page and size must be positive integers." });
            }

            try
            {
                var allHighlights = new List<Highlight>();
                foreach (var sport in _dataService.GetSports())
                {
                    var highlights = await _dataService.GetHighlightsAsync(sport);
                    allHighlights.AddRange(highlights);
                }

                var paginatedHighlights = allHighlights
                    .OrderBy(h => h.MatchDate) // Optional: sort by date
                    .Skip((page - 1) * size)
                    .Take(size);

                return Ok(paginatedHighlights);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while fetching highlights." });
            }
        }

        [HttpGet("{sport}/highlights")]
        public async Task<IActionResult> GetHighlights(string sport, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            if (string.IsNullOrWhiteSpace(sport) || !_dataService.IsValidSport(sport))
            {
                return BadRequest(new { Error = "Invalid or unsupported sport." });
            }
            if (page < 1 || size < 1)
            {
                return BadRequest(new { Error = "Page and size must be positive integers." });
            }

            try
            {
                var highlights = await _dataService.GetHighlightsAsync(sport, page, size);
                return Ok(highlights);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while fetching highlights." });
            }
        }

        [HttpGet("{sport}/schedule")]
        public async Task<IActionResult> GetSchedule(string sport, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            if (string.IsNullOrWhiteSpace(sport) || !_dataService.IsValidSport(sport))
            {
                return BadRequest(new { Error = "Invalid or unsupported sport." });
            }
            if (page < 1 || size < 1)
            {
                return BadRequest(new { Error = "Page and size must be positive integers." });
            }

            try
            {
                var schedule = await _dataService.GetScheduleAsync(sport, page, size);
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while fetching schedule." });
            }
        }
    }
}