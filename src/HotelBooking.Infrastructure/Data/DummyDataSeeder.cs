using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Data
{
    public class DummyDataSeeder : IDataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DummyDataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task ResetDataAsync()
        {
            _context.Bookings.RemoveRange(_context.Bookings);
            _context.Rooms.RemoveRange(_context.Rooms);
            _context.Hotels.RemoveRange(_context.Hotels);

            await _context.SaveChangesAsync();
        }

        public async Task SeedDataAsync()
        {
            //exit if data already exists
            if (await _context.Hotels.AnyAsync())
            {
                return;
            }

            var hotel = new Hotel
            {
                Id = Guid.NewGuid(),
                Name = "The Best Hotel",
                City = "London",
                Rooms =
                [
                    // 2 Single rooms with capacity 1
                    new Room
                    {
                        Id = Guid.NewGuid(),
                        RoomNumber = "101",
                        RoomType = RoomType.Single,
                        Capacity = 1,
                        PricePerNight = 80.00m
                    },
                    new Room
                    {
                        Id = Guid.NewGuid(),
                        RoomNumber = "102",
                        RoomType = RoomType.Single,
                        Capacity = 1,
                        PricePerNight = 80.00m
                    },
              
                    // 2 Double rooms with capacity: 2
                    new Room
                    {
                        Id = Guid.NewGuid(),
                        RoomNumber = "103",
                        RoomType = RoomType.Double,
                        Capacity = 2,
                        PricePerNight = 120.00m
                    },
                    new Room
                    {
                        Id = Guid.NewGuid(),
                        RoomNumber = "104",
                        RoomType = RoomType.Double,
                        Capacity = 2,
                        PricePerNight = 120.00m
                    },

                    // 2 Deluxe rooms (capacity: 4)
                    new Room
                    {
                        Id = Guid.NewGuid(),
                        RoomNumber = "105",
                        RoomType = RoomType.Deluxe,
                        Capacity = 4,
                        PricePerNight = 200.00m
                    },
                    new Room
                    {
                        Id = Guid.NewGuid(),
                        RoomNumber = "106",
                        RoomType = RoomType.Deluxe,
                        Capacity = 4,
                        PricePerNight = 200.00m
                    }
                ]
            };
            await _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync();


            var room1 = hotel.Rooms.ElementAt(0); // single room
            var room2 = hotel.Rooms.ElementAt(2); // double room
            var sampleBookings = new List<Booking>
            {
                new() {
                    Id = Guid.NewGuid(),
                    BookingReference = "BK-20251027-TEST",
                    HotelId = hotel.Id,
                    RoomId = room1.Id, 
                    CheckInDate = DateTime.Today.AddDays(7),
                    CheckOutDate = DateTime.Today.AddDays(10),
                    NumberOfGuests = 1,
                    GuestName = "Mr Testy",
                    TotalPrice = room1.PricePerNight* 3, 
                    BookDate = DateTime.UtcNow
                },
                new() {
                    Id = Guid.NewGuid(),
                    BookingReference = "BK-20251027-SAM1",
                    HotelId = hotel.Id,
                    RoomId = room2.Id, 
                    CheckInDate = DateTime.Today.AddDays(5),
                    CheckOutDate = DateTime.Today.AddDays(8),
                    NumberOfGuests = 2,
                    GuestName = "Mr Testerson",
                    TotalPrice = room2.PricePerNight * 3,
                    BookDate = DateTime.UtcNow
                }
            };


            _context.Bookings.AddRange(sampleBookings);
            await _context.SaveChangesAsync();
        }
    }
}
