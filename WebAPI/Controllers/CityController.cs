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
            //throw new UnauthorizedAccessException();
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,CityDto cityDto)
        {
            
            if(id != cityDto.Id){
                return BadRequest("Update not allowed");
            }
            var dbCity = await _unitOfWork.CityRepository.FindCity(id);
            if(dbCity == null){
                return BadRequest("Update not allowed");
            }
            dbCity!.LastUpdatedBy = 1;
            dbCity.LastUpdatedOn = DateTime.UtcNow;
            _mapper.Map(cityDto,dbCity);

            //throw new Exception("Some unknown error occured");
            await _unitOfWork.SaveAsync();
            return StatusCode(200);                        
        }

        //We will not use patch for any update request because of its flexibility to the users, moreover it is available with independent package "Newtonsoft.json".

        // [HttpPatch("{id}")]
        // public async Task<IActionResult> Patch(int id, JsonPatchDocument<City> cityToPatch)
        // {
        //     var dbCity = await _unitOfWork.CityRepository.FindCity(id);
        //     dbCity!.LastUpdatedBy = 1;
        //     dbCity.LastUpdatedOn = DateTime.UtcNow;

        //     cityToPatch.ApplyTo(dbCity, ModelState);
        //     await _unitOfWork.SaveAsync();
        //     return StatusCode(200);
        // }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            _unitOfWork.CityRepository.DeleteCity(id);            
            await _unitOfWork.SaveAsync();
            return Ok(id);
        }
        
    }
}