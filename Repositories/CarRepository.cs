using Car_Rental_System.Models;
using Car_Rental_System.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
namespace Car_Rental_System.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly IConfiguration _configuration;

        public CarRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            using var connection = GetConnection();

            string query = "SELECT * FROM Cars";

            return await connection.QueryAsync<Car>(query);
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            string query = "SELECT * FROM Cars WHERE Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<Car>(
                query, new { Id = id });
        }

        public async Task<int> AddAsync(Car car)
        {
            using var connection = GetConnection();

            string query = @"
                INSERT INTO Cars
                (CarName, Brand, Model, Year, PricePerDay,
                 ImageUrl, Description, IsAvailable)
                VALUES
                (@CarName, @Brand, @Model, @Year, @PricePerDay,
                 @ImageUrl, @Description, @IsAvailable);

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await connection.ExecuteScalarAsync<int>(query, car);
        }

        public async Task<bool> UpdateAsync(Car car)
        {
            using var connection = GetConnection();

            string query = @"
                UPDATE Cars SET
                CarName = @CarName,
                Brand = @Brand,
                Model = @Model,
                Year = @Year,
                PricePerDay = @PricePerDay,
                ImageUrl = @ImageUrl,
                Description = @Description,
                IsAvailable = @IsAvailable
                WHERE Id = @Id";

            int rows = await connection.ExecuteAsync(query, car);

            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = GetConnection();

            string query = "DELETE FROM Cars WHERE Id = @Id";

            int rows = await connection.ExecuteAsync(
                query, new { Id = id });

            return rows > 0;
        }
    }
}