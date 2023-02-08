using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.IRepository;

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
        
    }
}