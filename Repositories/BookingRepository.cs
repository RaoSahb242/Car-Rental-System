using Car_Rental_System.Models;
using Car_Rental_System.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
namespace Car_Rental_System.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IConfiguration _configuration;

        public BookingRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            using var connection = GetConnection();

            string query = "SELECT * FROM Bookings";

            return await connection.QueryAsync<Booking>(query);
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            string query = "SELECT * FROM Bookings WHERE Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<Booking>(
                query, new { Id = id });
        }

        public async Task<int> AddAsync(Booking booking)
        {
            using var connection = GetConnection();

            string query = @"
                INSERT INTO Bookings
                (CarId, CustomerId, BookingDate, StartDate,
                 EndDate, TotalAmount, Status)
                VALUES
                (@CarId, @CustomerId, @BookingDate, @StartDate,
                 @EndDate, @TotalAmount, @Status);

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await connection.ExecuteScalarAsync<int>(
                query, booking);
        }

        public async Task<bool> UpdateAsync(Booking booking)
        {
            using var connection = GetConnection();

            string query = @"
                UPDATE Bookings SET
                CarId = @CarId,
                CustomerId = @CustomerId,
                StartDate = @StartDate,
                EndDate = @EndDate,
                TotalAmount = @TotalAmount,
                Status = @Status
                WHERE Id = @Id";

            int rows = await connection.ExecuteAsync(query, booking);

            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = GetConnection();

            string query = "DELETE FROM Bookings WHERE Id = @Id";

            int rows = await connection.ExecuteAsync(
                query, new { Id = id });

            return rows > 0;
        }
    }
}