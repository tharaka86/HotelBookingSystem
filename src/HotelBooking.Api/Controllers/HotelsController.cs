using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly IBookingService _bookingService;
        private readonly ILogger<HotelsController> _logger;

        public HotelsController(IHotelService hotelService, IBookingService bookingService,ILogger<HotelsController> logger)
        {
            _hotelService = hotelService;
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetHotels(CancellationToken cancellationToken)
        {
            try
            {
                var hotels = await _hotelService.GetAll(cancellationToken);
                return Ok(hotels);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all hotels");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        // <summary>
        /// Find a hotel by its name
        /// </summary>
        /// <param name="request">Room availability search criteria</param>
        /// <returns>List of available rooms</returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> GetHotelByName(string name, CancellationToken cancellationToken)
        {
            /***************************************************************************************************
            Keeping exception handling in controller for now to return appropriate status codes
            this can be improved using middleware for global exception handling with custom exceptions as per need         
            ***************************************************************************************************/

            try
            {
                var hotel = await _hotelService.GetHotelByNameAsync(name, cancellationToken);
                if (hotel == null)
                {
                    return NotFound(new { message = $"No hotel found with name {name}" });
                }
                return Ok(hotel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting hotel by name: {HotelName}", name);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Find available rooms in a hotel based on criteria
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("rooms/available")]
        public async Task<IActionResult> GetAvailableRooms([FromBody] RoomAvailabilityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var availableRooms = await _bookingService.GetAvailableRoomsAsync(request, cancellationToken);
                return Ok(availableRooms);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid booking request");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting hotel by id: {HotelId}", "");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
