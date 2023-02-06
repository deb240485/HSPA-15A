using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Dtos;
using WebAPI.IRepository;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class AccountController : BaseController
    {
       private readonly IUnitOfWork _unitOfWork;
       private readonly IConfiguration _configuration;
       public AccountController(IUnitOfWork unitOfWork,IConfiguration configuration)
       {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
       }

       [HttpPost("Register")]
       public async Task<IActionResult> Register(LoginReqDto loginReq)
       {
            if(loginReq.userName == null || loginReq.userName == string.Empty)
                return BadRequest("UserName cannot be empty");
            if(await _unitOfWork.UserRepository.UserAlreadyExists(loginReq.userName!))
                return BadRequest("User already exists , please try different User Name.");
            
            _unitOfWork.UserRepository.Register(loginReq.userName!,loginReq.password!);

            await _unitOfWork.SaveAsync();
            return StatusCode(201);
       }

       [HttpPost("Login")]
       public async Task<IActionResult> Login(LoginReqDto loginReq){
            var user = await _unitOfWork.UserRepository.Authenticate(loginReq.userName!,loginReq.password!);

            if(user == null){
                return Unauthorized("Invalid User Name or Password");
            }

            var loginRespDto = new LoginRespDto();
            loginRespDto.userName = user.userName;
            loginRespDto.Token = CreateJWT(user);

            return Ok(loginRespDto);
        }

        private string CreateJWT(User user)
        {
            //var secretKey = _configuration.GetSection("AppSettings:Key").Value;
            // Using symetric key in this.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings").GetChildren().FirstOrDefault(j => j.Key == "key")!.Value
            ));           

            var claims = new Claim[]{
                new Claim(ClaimTypes.Name , user.userName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var signingCredentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256Signature);

            
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        } 
    }
}