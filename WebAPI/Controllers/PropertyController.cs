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
        public async Task<IActionResult> Photo(IFormFile file, int id){
            var result = await _photoService.UploadPhotoAsync(file);
            if(result.Error != null)
                return BadRequest(result.Error.Message);

            var property = await _unitOfWork.propertyRepository.GetPropertyPhotoAsync(id);
            var photo = new Photo {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                LastUpdatedBy = GetUserId()
            };

            if(property?.Photos!.Count == 0){
                photo.IsPrimary = true;
            }

            property?.Photos!.Add(photo);

            await _unitOfWork.SaveAsync();
            return StatusCode(201);
        }

        [HttpPost("primaryphoto/{id}/{publicId}")]
        [Authorize]
        public async Task<IActionResult> PrimaryPhoto(int id, string publicId){
            var userId = GetUserId();

            var property = await _unitOfWork.propertyRepository.GetPropertyAsync(id);

            if(property == null)
                return BadRequest("No such property or photo exists");

            if(property.PostedBy != userId)
                return BadRequest("You are not authorized to set primary photo");
            
            var photo = property.Photos!.FirstOrDefault(p => p.PublicId == publicId);

            if(photo == null)
                return BadRequest("No such property or photo exists");

            if(photo.IsPrimary)
                return BadRequest("This is already a primary photo");
            
            var currentPrimary = property.Photos!.FirstOrDefault(p => p.IsPrimary);
            if(currentPrimary != null) currentPrimary.IsPrimary = false;

            photo.IsPrimary = true;

            if(await _unitOfWork.SaveAsync()) return NoContent();

            return BadRequest("Some error has occured, failed to set primary photo");
        }

        [HttpDelete("delete/{id}/{publicId}")]
        [Authorize]
        public async Task<IActionResult> photo(int id, string publicId){
            var userId = GetUserId();

            var property = await _unitOfWork.propertyRepository.GetPropertyAsync(id);

            if(property == null)
                return BadRequest("No such property or photo exists");

            if(property.PostedBy != userId)
                return BadRequest("You are not authorized to delete photo");
            
            var photo = property.Photos!.FirstOrDefault(p => p.PublicId == publicId);

            if(photo == null)
                return BadRequest("No such property or photo exists");

            if(photo.IsPrimary)
                return BadRequest("You can not delete primary photo");

            var result = await _photoService.DeletePhotoAsync(photo.PublicId!);
            if(result.Error != null) return BadRequest(result.Error.Message);

            property.Photos!.Remove(photo);

            if(await _unitOfWork.SaveAsync()) return Ok();

            return BadRequest("Some error has occured, failed to delete photo");
        }
    }
}