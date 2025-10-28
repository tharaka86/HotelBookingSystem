using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces;

namespace HotelBooking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;
        public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository, IHotelRepository hotelRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
        }        

        public async Task<BookingDto> CreateBookingAsync(BookingRequest request, CancellationToken cancellationToken)
        {
            if (request.CheckInDate >= request.CheckOutDate)
                throw new ArgumentException("Check-out date must be after check-in date");

            if (request.CheckInDate < DateTime.Today)
                throw new ArgumentException("Check-in date cannot be in the past");

            if(string.IsNullOrWhiteSpace(request.GuestName))
                throw new ArgumentException("Guest name is required");

            _= await _hotelRepository.GetByIdAsync(request.HotelId, cancellationToken) ?? throw new ArgumentException("Hotel not found");
            var room = await _roomRepository.GetByIdAsync(request.RoomId, cancellationToken) ?? throw new ArgumentException("Room not found");

            if (request.NumberOfGuests > room.Capacity)
                throw new ArgumentException($"Room capacity is {room.Capacity}, but {request.NumberOfGuests} guests requested");

            //make sure room is available for the requested dates
            var isRoomAvailable = await _bookingRepository.IsRoomAvailableAsync(request.RoomId, request.CheckInDate, request.CheckOutDate, cancellationToken);
           
            if (!isRoomAvailable)
                throw new InvalidOperationException("Room is not available for the selected dates");


            var totalNights = (request.CheckOutDate - request.CheckInDate).Days;
            var totalPrice = totalNights * room.PricePerNight;

            var bookingReference = GenerateUniqueBookingReference();

            var booking = new Booking
            {
                BookingReference = bookingReference,
                HotelId = request.HotelId,
                RoomId = request.RoomId,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                NumberOfGuests = request.NumberOfGuests,
                GuestName = request.GuestName,
                TotalPrice = totalPrice,
                BookDate = DateTime.UtcNow
            };

            var createdBooking = await _bookingRepository.AddAsync(booking,cancellationToken);

            return MapToBookingDto(createdBooking);
        }


        public async Task<List<RoomDto>> GetAvailableRoomsAsync(RoomAvailabilityRequest request, CancellationToken cancellationToken)
        {
            // keeping input validation here for simplicity, but can be use fluent validation or IValidatableObject etc..
            if (request.CheckInDate >= request.CheckOutDate)
                throw new ArgumentException("Check-out date must be after check-in date");

            if (request.CheckInDate < DateTime.Today)
                throw new ArgumentException("Check-in date cannot be in the past");

            if (request.NumberOfGuests <= 0)
                throw new ArgumentException("Number of guests must be greater than 0");

            var availableRooms = await _roomRepository.GetAvailableRoomsAsync(
                                    request.HotelId,
                                    request.CheckInDate,
                                    request.CheckOutDate,
                                    request.NumberOfGuests,
                                    cancellationToken);

            return [.. availableRooms.Select(r => new RoomDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,               
                RoomType = r.RoomType.ToString(),
                Capacity = r.Capacity,
                PricePerNight = r.PricePerNight
            })];
        }

        public async Task<List<BookingDto>> GetBookingsByHotelAsync(Guid hotelId, CancellationToken cancellationToken)
        {
            var bookings = await _bookingRepository.GetByHotelAsync(hotelId, cancellationToken);
            return [.. bookings.Select(MapToBookingDto)];
        }

        public async Task<BookingDto?> GetBookingByReferenceAsync(string bookingReference, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByReferenceAsync(bookingReference, cancellationToken);
            if(booking == null)
            {
                return null;
            }

            return MapToBookingDto(booking);
        }

        private static BookingDto MapToBookingDto(Booking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                BookingReference = booking.BookingReference,
                HotelName = booking.Hotel.Name,
                RoomNumber = booking.Room.RoomNumber,
                RoomType = booking.Room.RoomType.ToString(),
                NumberOfGuests = booking.NumberOfGuests,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                GuestName = booking.GuestName,
                TotalPrice = booking.TotalPrice,
                BookDate = booking.BookDate,
            };
        }

        /// <summary>
        /// Generate a unique booking reference
        /// This is a simple implementation, but ideally want to ensure uniqueness in the database 
        /// </summary>
        /// <returns></returns>
        private static string GenerateUniqueBookingReference()
        {
            return $"BK-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }


    }
}
