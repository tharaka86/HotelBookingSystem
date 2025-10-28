using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Interfaces;

namespace HotelBooking.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<List<HotelDto>> GetAll(CancellationToken cancellationToken)
        {
            var hotels = await _hotelRepository.GetAllAsync(cancellationToken);
            return [.. hotels.Select(MapToHotelDto)];
        }

        public async Task<HotelDto?> GetHotelByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id, cancellationToken);
            if (hotel == null)
            {
                return null;
            }

            return MapToHotelDto(hotel);
        }

        public async Task<HotelDto?> GetHotelByNameAsync(string name, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetByNameAsync(name, cancellationToken);
            if (hotel == null)
            {
                return null;
            }

            return MapToHotelDto(hotel);
        }

        private static HotelDto MapToHotelDto(Domain.Entities.Hotel hotel)
        {
            return new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                City = hotel.City,
                TotalRooms = hotel.Rooms?.Count ?? 0
            };
        }

       
    }
}
