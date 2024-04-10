using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ContactManager.Context;
using ContactManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using ContactManager.helpers;

namespace ContactManager.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly AppDbContext _context;

        public AuthController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var userDB = _context.Users.FirstOrDefault(u => u.UserName == user.UserName);
            if (userDB != null)
            {
                return BadRequest("Username already exist. Please choose another.");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            // var ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var ip = "152.207.144.125";  // IP de Cuba CU
            // var ip = "35.232.52.59";     // IP de United States US
            HttpClient client = new HttpClient();
            var respuesta = await client.GetStringAsync("https://www.iplocate.io/api/lookup/" + ip); 
            var dataIP = JsonSerializer.Deserialize<DataIP>(respuesta);

            var user = _context.Users.SingleOrDefault(u => u.UserName == userLogin.UserName && u.Password == userLogin.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Incorrect username or password." });
            }

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
            var token = HelperJwt.GenerateJwtToken(user, jwt.Key, dataIP.country_code);
            return Ok(new { Token = token });
        }
    }
}