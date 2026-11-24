using Car_Rental_System.Models;
using Car_Rental_System.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
namespace Car_Rental_System.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IConfiguration _configuration;

        public PaymentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            using var connection = GetConnection();

            string query = "SELECT * FROM Payments";

            return await connection.QueryAsync<Payment>(query);
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            string query = "SELECT * FROM Payments WHERE Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<Payment>(
                query, new { Id = id });
        }

        public async Task<int> AddAsync(Payment payment)
        {
            using var connection = GetConnection();

            string query = @"
                INSERT INTO Payments
                (BookingId, Amount, PaymentMethod, PaymentStatus, PaymentDate)
                VALUES
                (@BookingId, @Amount, @PaymentMethod, @PaymentStatus, @PaymentDate);

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await connection.ExecuteScalarAsync<int>(
                query, payment);
        }

        public async Task<bool> UpdateAsync(Payment payment)
        {
            using var connection = GetConnection();

            string query = @"
                UPDATE Payments SET
                BookingId = @BookingId,
                Amount = @Amount,
                PaymentMethod = @PaymentMethod,
                PaymentStatus = @PaymentStatus
                WHERE Id = @Id";

            int rows = await connection.ExecuteAsync(query, payment);

            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = GetConnection();

            string query = "DELETE FROM Payments WHERE Id = @Id";

            int rows = await connection.ExecuteAsync(
                query, new { Id = id });

            return rows > 0;
        }
    }
}