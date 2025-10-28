using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IBookingService bookingService, ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetBookingsByHotel(Guid hotelId, CancellationToken cancellationToken)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByHotelAsync(hotelId, cancellationToken);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting bookings for hotel: {HotelId}", hotelId);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        /// <summary>
        /// Get booking details by booking reference
        /// </summary>
        /// <param name="bookingReference"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{bookingReference}")]
        public async Task<IActionResult> GetBookingByReference(string bookingReference, CancellationToken cancellationToken)
        {
            try
            {
                var booking = await _bookingService.GetBookingByReferenceAsync(bookingReference, cancellationToken);
                if (booking == null)
                {
                    return NotFound(new { message = $"No booking found with reference {bookingReference}" });
                }
                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting booking by reference: {BookingReference}", bookingReference);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Create a new booking
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingRequest request, CancellationToken cancellationToken)
        {          
            try
            {
                var booking = await _bookingService.CreateBookingAsync(request, cancellationToken);
                return CreatedAtAction(nameof(GetBookingByReference), new { bookingReference = booking.BookingReference }, booking);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid booking request");
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Create booking failed");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating booking.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

    }
}
