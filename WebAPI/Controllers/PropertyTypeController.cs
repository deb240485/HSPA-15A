using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.IRepository;

namespace WebAPI.Controllers
{
    public class PropertyTypeController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PropertyTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
          _unitOfWork = unitOfWork;
          _mapper = mapper;  
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PropertyTypes(){
            
            var propertyTypes = await _unitOfWork.propertyTypeRepository.GetPropertyTypeAsync();
            var PropertyTypesDto = _mapper.Map<IEnumerable<KeyValuePairDto>>(propertyTypes);
            return Ok(PropertyTypesDto);
        }
    }
}