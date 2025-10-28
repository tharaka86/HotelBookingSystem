namespace HotelBooking.Application.DTOs
{
    public class BookingRequest
    {
        public Guid HotelId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string GuestName { get; set; } = string.Empty;
    }
}
