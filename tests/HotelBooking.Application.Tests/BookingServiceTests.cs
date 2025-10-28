using FluentAssertions;
using HotelBooking.Application.DTOs;
using HotelBooking.Application.Services;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using HotelBooking.Domain.Interfaces;
using Moq;

namespace HotelBooking.Application.Tests
{ 
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _mockBookingRepository;
        private readonly Mock<IRoomRepository> _mockRoomRepository;
        private readonly Mock<IHotelRepository> _mockHotelRepository;
        private readonly BookingService _bookingService;

        public BookingServiceTests()
        {
            _mockBookingRepository = new Mock<IBookingRepository>();
            _mockRoomRepository = new Mock<IRoomRepository>();
            _mockHotelRepository = new Mock<IHotelRepository>();

            _bookingService = new BookingService(
                _mockBookingRepository.Object,
                _mockRoomRepository.Object,
                _mockHotelRepository.Object);
        }


        /// <summary>
        /// Make sure that an attempt to create a booking for a room that already has an overlapping booking throws an InvalidOperationException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateBookingAsync_WhenRoomHasOverlappingBooking_ThrowsInvalidOperationException()
        {
            // Arrange

            var hotelId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var request = new BookingRequest
            {
                HotelId = hotelId,
                RoomId = roomId,
                CheckInDate = new DateTime(2025, 11, 15),
                CheckOutDate = new DateTime(2025, 11, 18),
                NumberOfGuests = 2,
                GuestName = "Mr Testy",
            }; 

            var room = new Room
            {
                Id = roomId,
                HotelId = hotelId,
                RoomNumber = "201",
                RoomType = RoomType.Double,
                Capacity = 2,
                PricePerNight = 120.00m
            };
            var hotel = new Hotel
            {
                Id = hotelId,
                Name = "Test Hotel",               
                City = "Test City"
            };

            _mockRoomRepository
             .Setup(repo => repo.GetByIdAsync(roomId, cancellationToken))
             .ReturnsAsync(room);

            _mockHotelRepository
            .Setup(repo => repo.GetByIdAsync(hotelId, cancellationToken))
            .ReturnsAsync(hotel);

            // Mock to return false for room avvailability check to simulate overlapping booking
            _mockBookingRepository
                .Setup(repo => repo.IsRoomAvailableAsync(
                    request.RoomId,
                    request.CheckInDate,
                    request.CheckOutDate,
                    cancellationToken))
                .ReturnsAsync(false);

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _bookingService.CreateBookingAsync(request,cancellationToken));


            // Assert
            exception.Message.Should().Be("Room is not available for the selected dates");

            // make sure availability was checked
            _mockBookingRepository.Verify(
                repo => repo.IsRoomAvailableAsync(request.RoomId, request.CheckInDate, request.CheckOutDate, cancellationToken),
                Times.Once,
                "Service must check room availability before creating booking");

            // check booking was NOT created
            _mockBookingRepository.Verify(
                repo => repo.AddAsync(It.IsAny<Booking>(), cancellationToken),
                Times.Never,
                "Booking should NOT be created when room is unavailable");
        }

    }
}
