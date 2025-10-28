using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetByHotelAsync(Guid hotelId, CancellationToken cancellationToken);
        Task<Booking> AddAsync(Booking booking, CancellationToken cancellationToken);
        Task<Booking?> GetByReferenceAsync(string bookingReference, CancellationToken cancellationToken);
        Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken);
    }
}
