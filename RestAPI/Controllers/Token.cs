using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Token : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public string Get()
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            string _privateKey = "12345678901234567890";
            var key = Encoding.ASCII.GetBytes(_privateKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                   //new Claim(ClaimTypes.Name, "")
                }),
                Issuer = "https://localhost:5002",
                Audience = "https://localhost:5001",
                Expires = DateTime.UtcNow.AddDays(7),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenValue = tokenHandler.WriteToken(token);
            return tokenValue;
        }


        [HttpGet("test")]        
        public IActionResult TestToken()
        {
            return Ok();
        }
    }
}
