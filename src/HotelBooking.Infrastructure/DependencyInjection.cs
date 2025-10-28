using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Interfaces;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //using in-memory database for simplicity
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("HotelBookingDb"));

            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped<IDataSeeder, DummyDataSeeder>();
            return services;
        }
    }
}
