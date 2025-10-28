using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly ApplicationDbContext _context;
        public HotelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Hotel>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Hotels
                .Include(h=>h.Rooms)
                .ToListAsync(cancellationToken);
        }

        public async Task<Hotel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<Hotel?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .FirstOrDefaultAsync(h => EF.Functions.Like(h.Name, name), cancellationToken);
        }

    }
}
