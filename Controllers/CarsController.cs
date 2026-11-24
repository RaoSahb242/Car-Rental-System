using Car_Rental_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_System.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarRepository _carRepository;
        public CarsController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _carRepository.GetAllAsync();

            return Json(cars);
        }
    }
}