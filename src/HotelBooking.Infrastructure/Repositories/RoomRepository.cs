using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetAvailableRoomsAsync(Guid hotelId, DateTime checkIn, DateTime checkOut, int numberOfGuests, CancellationToken cancellationToken)
        {
            var rooms = await _context.Rooms
                .Where(r => r.HotelId == hotelId && r.Capacity >= numberOfGuests).ToListAsync(cancellationToken);
            var availableRooms = new List<Room>();

            foreach (var room in rooms)
            {
                // Check if the room has any overlapping bookings
                var hasOverlappingBooking = await _context.Bookings
                    .AnyAsync(b => b.RoomId == room.Id &&
                        b.CheckInDate < checkOut && b.CheckOutDate > checkIn);

                if (!hasOverlappingBooking)
                {
                    availableRooms.Add(room);
                }
            }
            return availableRooms;
        }

        public Task<List<Room>> GetByHotelIdAsync(Guid hotelId)
        {
            throw new NotImplementedException();
        }

        public async Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r=> r.Id == id, cancellationToken);
        }
    }
}
