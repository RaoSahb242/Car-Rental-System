using Car_Rental_System.Models;
namespace Car_Rental_System.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync();

        Task<Payment?> GetByIdAsync(int id);

        Task<int> AddAsync(Payment payment);

        Task<bool> UpdateAsync(Payment payment);

        Task<bool> DeleteAsync(int id);
    }
}