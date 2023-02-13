using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.IRepository;

namespace WebAPI.Controllers
{
    public class FurnishingTypeController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FurnishingTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FurnishingType(){
            var furnishingType = await _unitOfWork.furnishingTypeRepository.FurnishingTypeAsync(); 
            var furnishingTypeDto = _mapper.Map<IEnumerable<KeyValuePairDto>>(furnishingType);
            return Ok(furnishingTypeDto);
        }
    }
}