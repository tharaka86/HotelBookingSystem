
namespace HotelBooking.Application.DTOs
{
    public class RoomAvailabilityRequest
    {
        public Guid HotelId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
