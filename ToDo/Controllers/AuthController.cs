using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using ToDo.Models;

namespace ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        //Injected to read Settings and Connection Strings from AppSettings.json
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static User user = new User();


        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        //User Registration
        public ActionResult<User> Register(UserDTO userDTO)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.password);
            if (userDTO.roles == "Admin" || userDTO.roles == "User")
            {
                user.firstName = userDTO.firstName;
                user.lastName = userDTO.lastName;
                user.email = userDTO.email;
                user.isActive = userDTO.isActive;
                user.roles = userDTO.roles;
                user.passwordHash = passwordHash;
            }
            else
            {
                return BadRequest("Roles of user can be either Admin or User");
            }
            return Ok(user);
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        //User login method
        public ActionResult<User> Login(UserDTO userDTO)
        {
            if (user.email != userDTO.email)
                return BadRequest("User not found");

            if (!BCrypt.Net.BCrypt.Verify(userDTO.password, user.passwordHash))
                return BadRequest("Invalid credentials");

            string token = CreateToken(user);

            return Ok(token);
        }

        //Creating JwT token for successful login
        private string CreateToken(User user)
        {
            List <Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.firstName),
                new Claim(ClaimTypes.Name, user.lastName),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Name, user.isActive),
                new Claim(ClaimTypes.Role, user.roles)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims : claims,
                expires : DateTime.Now.AddDays(1),
                signingCredentials:creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
