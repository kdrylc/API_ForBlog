using API_ForBlog.Data;
using API_ForBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace API_ForBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userMan;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(UserManager<User> userMan, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _userMan = userMan;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var response = new MainResponse();
            if (refreshTokenRequest is null)
            {
                response.ErrorMessage = "Invalid  request";
                return BadRequest(response);
            }

            var principal = GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken);

            if (principal != null)
            {
                var email = principal.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Email);

                var user = await _userMan.FindByEmailAsync(email?.Value);

                if (user is null || user.RefreshToken != refreshTokenRequest.RefreshToken)
                {
                    response.ErrorMessage = "Invalid Request";
                    return BadRequest(response);
                }

                string newAccessToken = GenerateAccessToken(user);
                string refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                await _userMan.UpdateAsync(user);

                response.IsSuccess = true;
                response.Content = new AuthenticationResponse
                {
                    RefreshToken = refreshToken,
                    AccessToken = newAccessToken
                };
                return Ok(response);
            }
            else
            {
                return ErrorResponse.ReturnErrorResponse("Invalid Token Found");
            }

        }



        [AllowAnonymous]
        [HttpPost("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser(AuthenticateUser authenticateUser)
        {
            var user = await _userMan.FindByNameAsync(authenticateUser.UserName);
            if (user == null) return Unauthorized();

            bool isValidUser = await _userMan.CheckPasswordAsync(user, authenticateUser.Password);

            if (isValidUser)
            {
                string accessToken = GenerateAccessToken(user);
                var refreshToken = GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                await _userMan.UpdateAsync(user);

                var response = new MainResponse
                {
                    Content = new AuthenticationResponse
                    {
                        RefreshToken = refreshToken,
                        AccessToken = accessToken
                    },
                    IsSuccess = true,
                    ErrorMessage = ""
                };
                return Ok(response);
            }
            else
            {
                return Unauthorized();
            }

        }

        private string GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var keyDetail = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} { user.LastName}"),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("UserAvatar", $"{user.UserAvatar}"),

            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _configuration["JWT:Audience"],
                Issuer = _configuration["JWT:Issuer"],
                Expires = DateTime.UtcNow.AddMinutes(30),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyDetail), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var keyDetail = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenValidationParameter = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidAudience = _configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(keyDetail),
            };

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameter, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        private string GenerateRefreshToken()
        {

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterUserDTO registerUserDTO)
        {

            var userToBeCreated = new User
            {
                Email = registerUserDTO.Email,
                FirstName = registerUserDTO.FirstName,
                LastName = registerUserDTO.LastName,
                UserName = registerUserDTO.Email,
                Address = registerUserDTO.Address,
                Gender = registerUserDTO.Gender,
                IsAdmin = false
            };


            if (!string.IsNullOrWhiteSpace(registerUserDTO.UserAvatar))
            {
                byte[] imgBytes = Convert.FromBase64String(registerUserDTO.UserAvatar);
                string fileName = $"{Guid.NewGuid()}_{userToBeCreated.FirstName.Trim()}_{userToBeCreated.LastName.Trim()}.jpeg";
                string avatar = await UploadFile(imgBytes, fileName);
                userToBeCreated.UserAvatar = avatar;
            }

            var response = await _userMan.CreateAsync(userToBeCreated, registerUserDTO.Password);
            if (response.Succeeded)
            {
                return Ok(new MainResponse
                {
                    IsSuccess = true,
                });
            }
            else
            {
                return ErrorResponse.ReturnErrorResponse(response.Errors?.ToString() ?? "");
            }
        }

        private async Task<string> UploadFile(byte[] bytes, string fileName)
        {
            string uploadsFolder = Path.Combine("Images", fileName);
            Stream stream = new MemoryStream(bytes);
            using (var ms = new FileStream(uploadsFolder, FileMode.Create))
            {
                await stream.CopyToAsync(ms);
            }
            return uploadsFolder;
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(DeleteUserDTO userDetails)
        {

            var existingUser = await _userMan.FindByEmailAsync(userDetails.Email);
            if (existingUser != null)
            {
                var response = await _userMan.DeleteAsync(existingUser);

                if (response.Succeeded)
                {
                    return Ok(new MainResponse
                    {
                        IsSuccess = true,
                    });
                }
                else
                {
                    return ErrorResponse.ReturnErrorResponse(response.Errors?.ToString() ?? "");
                }
            }
            else
            {
                return ErrorResponse.ReturnErrorResponse("No User found with this email");
            }
        }

    }
}
