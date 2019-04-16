using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DtApp.API.Data.DTOs;
using DtApp.API.Data.Repository;
using DtApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DtApp.API.Controllers 
{
    [Route ("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController (IAuthRepository repo, IConfiguration config) 
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost ("register")]
        public async Task<IActionResult> Register (UserForRegisterDto userForRegisterDto) 
        {
            userForRegisterDto.username = userForRegisterDto.username.ToLower ();

            if (await _repo.UserExists (userForRegisterDto.username))
                return BadRequest ("Username already exists!");

            var userToCreate = new User {
                Username = userForRegisterDto.username
            };

            var createdUser = await _repo.Register (userToCreate, userForRegisterDto.password);

            return StatusCode (201); // Temporarily return type
        }

        [HttpPost ("login")]
        public async Task<IActionResult> Login (UserForLoginDto userForLoginDto) 
        {
            // Check for the user in the database
            var userFromRepo = await _repo.Login (userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            // if the user is not found return 
            if (userFromRepo == null)
                return Unauthorized ();

            // Build our token and key
            var claims = new [] {
                new Claim (ClaimTypes.NameIdentifier, userFromRepo.Id.ToString ()),
                new Claim (ClaimTypes.Name, userFromRepo.Username)
            };

            // To validate that the token is valid and make sure that it signed
            var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_config.GetSection("AppSettings:Token").Value));

            // We use that key as part of the Signing credentials and encrypting the key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Create our token. Security token decryptor that would contain our claims
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // Token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }

    }
}