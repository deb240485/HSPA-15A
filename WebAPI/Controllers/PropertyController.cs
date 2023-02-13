using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.IRepository;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PropertyController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PropertyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("list/{sellRent}")]
        [AllowAnonymous]
        public async Task<IActionResult> Properties(int sellRent)
        {
            var properties = await _unitOfWork.propertyRepository.GetPropertiesAsync(sellRent);
            var propertylistDTO =  _mapper.Map<IEnumerable<PropertyListDto>>(properties);
            return Ok(propertylistDTO);

        }
        
        [HttpGet("detail/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Property(int id){
            var property = await _unitOfWork.propertyRepository.GetPropertyAsync(id);
            var propertyDTO = _mapper.Map<PropertyDetailDto>(property);
            return Ok(propertyDTO);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Property(PropertyDto propertyDto){
            var property = _mapper.Map<Property>(propertyDto);
            var userId = GetUserId();
            property.PostedBy = userId;
            property.LastUpdatedBy = userId;

            _unitOfWork.propertyRepository.AddProperty(property);
            await _unitOfWork.SaveAsync();
            return StatusCode(201);
        }
    }
}