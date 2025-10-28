
namespace HotelBooking.Application.Interfaces
{
    public interface IDataSeeder
    {
        Task SeedDataAsync();
        Task ResetDataAsync();
    }
}
