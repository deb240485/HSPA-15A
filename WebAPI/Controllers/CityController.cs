using Microsoft.AspNetCore.Mvc;
using WebAPI.Data.Repository.IRepository;
using WebAPI.Models;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository _cityRepo;

        public CityController(ICityRepository cityRepo)
        {
            _cityRepo = cityRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cities = await _cityRepo.GetCitiesAsync();
            return Ok(cities);
        }

        [HttpPost]
        public async Task<IActionResult> Post(City city)
        {            
            _cityRepo.AddCity(city);
            await _cityRepo.SaveAsync();
            return StatusCode(201);            
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            _cityRepo.DeleteCity(id);            
            await _cityRepo.SaveAsync();
            return Ok(id);
        }
        
    }
}