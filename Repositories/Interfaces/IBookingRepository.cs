using Car_Rental_System.Models;
namespace Car_Rental_System.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllAsync();

        Task<Booking?> GetByIdAsync(int id);

        Task<int> AddAsync(Booking booking);

        Task<bool> UpdateAsync(Booking booking);

        Task<bool> DeleteAsync(int id);
    }
}