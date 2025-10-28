using HotelBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataAdminController : ControllerBase
    {
        private readonly IDataSeeder _dataSeeder;
        private readonly ILogger<DataAdminController> _logger;

        public DataAdminController(IDataSeeder dataSeeder, ILogger<DataAdminController> logger)
        {
            _dataSeeder = dataSeeder;
            _logger = logger;
        }

        /// <summary>
        /// Populate database with dummy data
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("seed")]
        public async Task<IActionResult> SeedData(CancellationToken cancellationToken)
        {
            try
            {
                await _dataSeeder.SeedDataAsync();
                return Ok(new { message = "Data seeded successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while seeding data.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Clear all data from the database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("reset")]
        public async Task<IActionResult> Reset(CancellationToken cancellationToken)
        {
            try
            {
                await _dataSeeder.ResetDataAsync();
                return Ok(new { message = "Data reset successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while resetting data.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
