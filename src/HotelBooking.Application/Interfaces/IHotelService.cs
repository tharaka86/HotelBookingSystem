using HotelBooking.Application.DTOs;

namespace HotelBooking.Application.Interfaces
{
    public interface IHotelService
    {
        Task<List<HotelDto>> GetAll(CancellationToken cancellationToken);
        Task<HotelDto?> GetHotelByNameAsync(string name, CancellationToken cancellationToken);
        Task<HotelDto?> GetHotelByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
