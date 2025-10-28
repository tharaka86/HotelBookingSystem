﻿namespace HotelBooking.Application.DTOs
{
    public class HotelDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int TotalRooms { get; set; }
    }
}
