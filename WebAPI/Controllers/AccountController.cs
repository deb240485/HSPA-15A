using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Dtos;
using WebAPI.Errors;
using WebAPI.Extensions;
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
            ApiError apiError = new ApiError();

            if(loginReq.userName!.IsEmpty()|| loginReq.password!.IsEmpty()){
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "UserName or password cannot be empty";
                return BadRequest(apiError);
            }

            if(await _unitOfWork.UserRepository.UserAlreadyExists(loginReq.userName!)){

                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "User already exists , please try different User Name."; 
                return BadRequest(apiError);
            }    
            
            _unitOfWork.UserRepository.Register(loginReq.userName!,loginReq.password!);

            await _unitOfWork.SaveAsync();
            return StatusCode(201);
       }

       [HttpPost("Login")]
       public async Task<IActionResult> Login(LoginReqDto loginReq){
            var user = await _unitOfWork.UserRepository.Authenticate(loginReq.userName!,loginReq.password!);

            ApiError apiError = new ApiError();

            if(user == null){
                apiError.ErrorCode = Unauthorized().StatusCode;
                apiError.ErrorMessage = "Invalid User Name or Password";
                apiError.ErrorDetails = "Provided User Name or Password does not exists";
                return Unauthorized(apiError);
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
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        } 
    }
}