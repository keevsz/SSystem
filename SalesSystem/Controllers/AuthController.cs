using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SalesSystem.Context;
using SalesSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SalesSystem.Controllers
{

    [ApiController]
    [Route("auth")]
    public class AuthController: ControllerBase
    {
        private readonly ApplicationDBContext DB;

        public IConfiguration _config;
        public AuthController(IConfiguration configuration, ApplicationDBContext context)
        {
            DB = context;
            _config = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<dynamic> login([FromBody] Object opData)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(opData.ToString()!);
            if (data == null)
            {
                return new
                {
                    success = false,
                    message = "Data not valid",
                    result = ""
                };
            }
            string username = data.username.ToString();
            string password = data.password.ToString();


            var user = await DB.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return new
                {
                    success = false,
                    message = "User not found",
                    result = ""
                };
            }
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (isValidPassword == false)
            {
                return new
                {
                    success = false,
                    message = "Invalid credentials",
                    result = ""
                };
            }


            var jwt = _config.GetSection("Jwt");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
           {
                new Claim("id", user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var token = new JwtSecurityToken(
               _config["Jwt:Issuer"],
               _config["Jwt:Audience"],
               claims,
               expires: DateTime.Now.AddMinutes(60),
               signingCredentials: credentials
            );

            return new
            {
                success = true,
                message = "Ok",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

       
    }
}
