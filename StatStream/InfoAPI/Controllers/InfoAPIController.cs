using InfoAPI.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace InfoAPI.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class InfoController : ControllerBase
    {
        private readonly ISportsDataService _dataService;

        public InfoController(ISportsDataService dataService)
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
                var highlights = await _dataService.GetAllHighlightsAsync(page, size);
                var totalCount = _dataService.GetTotalHighlightsCount();
                return Ok(new { Highlights = highlights, TotalCount = totalCount });
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

        [HttpGet("schedules")]
        public async Task<IActionResult> GetAllSchedules([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            if (page < 1 || size < 1)
            {
                return BadRequest(new { Error = "Page and size must be positive integers." });
            }

            try
            {
                var schedules = await _dataService.GetAllSchedulesAsync(page, size);
                var totalCount = _dataService.GetTotalSchedulesCount();
                return Ok(new { Schedules = schedules, TotalCount = totalCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while fetching schedules." });
            }
        }
    }
}