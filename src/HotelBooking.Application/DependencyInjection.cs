using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register Application Services
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IBookingService, BookingService>();

            return services;
        }
    }
}
