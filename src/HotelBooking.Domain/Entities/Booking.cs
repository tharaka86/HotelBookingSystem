using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }
        public string BookingReference { get; set; } = string.Empty;
        public Guid HotelId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTime BookDate { get; set; }

        public Hotel Hotel { get; set; } = null!;
        public Room Room { get; set; } = null!;
    }
}
