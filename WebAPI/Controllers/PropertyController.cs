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

        private readonly IPhotoService _photoService;
        public PropertyController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
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

        [HttpPost("photo/{id}")]
        [Authorize]
        public async Task<IActionResult> Photo(IFormFile file, int propId){
            var result = await _photoService.UploadPhotoAsync(file);
            if(result.Error != null)
                return BadRequest(result.Error.Message);

            return Ok(201);
        }
    }
}