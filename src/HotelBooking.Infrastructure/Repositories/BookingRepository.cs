using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> AddAsync(Booking booking, CancellationToken cancellationToken)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(cancellationToken);
            return await GetByIdAsync(booking.Id) ?? booking;
        }

        public async Task<List<Booking>> GetByHotelAsync(Guid hotelId, CancellationToken cancellationToken)
        {
            return await _context.Bookings
               .Include(h => h.Hotel)
               .Include(r => r.Room)
               .Where(b => b.HotelId == hotelId)
               .ToListAsync(cancellationToken);
        }

        public async Task<Booking?> GetByReferenceAsync(string bookingReference, CancellationToken cancellationToken)
        {
            return await _context.Bookings
                .Include(h => h.Hotel)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(b => b.BookingReference == bookingReference, cancellationToken);
        }

        public async Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken)
        {
            var overlappingBookings = await _context.Bookings
                .Where(b => b.RoomId == roomId &&
                            b.CheckInDate < checkOut && b.CheckOutDate > checkIn)
                .ToListAsync(cancellationToken);
            return overlappingBookings.Count == 0;
        }

        private async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _context.Bookings
                .Include(b => b.Hotel)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
