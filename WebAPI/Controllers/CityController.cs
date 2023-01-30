using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.IRepository;
using WebAPI.Models;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CityController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cities = await _unitOfWork.CityRepository.GetCitiesAsync();
            var citiesDto = _mapper.Map<IEnumerable<CityDto>>(cities);
            return Ok(citiesDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CityDto cityDto)
        {   
            var city = _mapper.Map<City>(cityDto);
            city.LastUpdatedBy = 1;
            city.LastUpdatedOn = DateTime.UtcNow;
                    
            _unitOfWork.CityRepository.AddCity(city);
            await _unitOfWork.SaveAsync();
            return StatusCode(201);            
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            _unitOfWork.CityRepository.DeleteCity(id);            
            await _unitOfWork.SaveAsync();
            return Ok(id);
        }
        
    }
}