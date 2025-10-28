using HotelBooking.Domain.Entities;


namespace HotelBooking.Domain.Interfaces
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAllAsync(CancellationToken cancellationToken);
        Task<Hotel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Hotel?> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
