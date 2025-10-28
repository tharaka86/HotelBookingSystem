using HotelBooking.Domain.Enums;

namespace HotelBooking.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public RoomType RoomType { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public Hotel Hotel { get; set; } = null!;
        public ICollection<Booking> Bookings { get; set; } = [];
    }
}
