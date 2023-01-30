using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
//using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly DataContext _conContext;
        public CityController(DataContext conContext)
        {
            _conContext = conContext;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            var cities = _conContext.Cities?.ToList();
            return Ok(cities);
        }

        
    }
}