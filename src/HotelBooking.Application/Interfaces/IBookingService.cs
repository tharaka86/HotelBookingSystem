using HotelBooking.Application.DTOs;


namespace HotelBooking.Application.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingDto>> GetBookingsByHotelAsync(Guid hotelId, CancellationToken cancellationToken);
        Task<List<RoomDto>> GetAvailableRoomsAsync(RoomAvailabilityRequest request, CancellationToken cancellationToken);
        Task<BookingDto> CreateBookingAsync(BookingRequest request, CancellationToken cancellationToken);
        Task<BookingDto?> GetBookingByReferenceAsync(string bookingReference, CancellationToken cancellationToken);
    }
}
