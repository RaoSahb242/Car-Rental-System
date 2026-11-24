using Car_Rental_System.Models;
namespace Car_Rental_System.Repositories.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllAsync();

        Task<Car?> GetByIdAsync(int id);

        Task<int> AddAsync(Car car);

        Task<bool> UpdateAsync(Car car);

        Task<bool> DeleteAsync(int id);
    }
}