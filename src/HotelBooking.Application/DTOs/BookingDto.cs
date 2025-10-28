
namespace HotelBooking.Application.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public string BookingReference { get; set; } = string.Empty;
        public string HotelName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTime BookDate { get; set; }
    }
}
