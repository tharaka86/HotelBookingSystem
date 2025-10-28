﻿
namespace HotelBooking.Domain.Entities
{
    public class Hotel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public ICollection<Room> Rooms { get; set; } = [];
        public ICollection<Booking> Bookings { get; set; } = [];
    }
}
