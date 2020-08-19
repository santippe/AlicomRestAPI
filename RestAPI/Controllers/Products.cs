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
    public class Product : ControllerBase
    {
        [HttpGet("product/{supplier}/{code}")]
        public IActionResult Get()
        {

            return Ok();
        }


        [HttpPost("product/{supplier}/{code}")]
        public void Get(dynamic itemToAdd)
        {

        }
    }
}
