using HotelBooking.Domain.Entities;

namespace HotelBooking.Domain.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Room>> GetByHotelIdAsync(Guid hotelId);
        Task<List<Room>> GetAvailableRoomsAsync(Guid hotelId, DateTime checkIn, DateTime checkOut, int numberOfGuests, CancellationToken cancellationToken);
    }
}
