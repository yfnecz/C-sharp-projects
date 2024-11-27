using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using backend.Helpers;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Users : ControllerBase
    {
        private ApiContext _apiContext;
        private JwtOptions _jwtOptions;

        public Users(ApiContext context, IOptions<JwtOptions> jwtOptions){
            _apiContext = context;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpGet("/getUserById/{id}")]
        public async Task<ActionResult<string>> GetUser(int id)
        {
            var user = await _apiContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.Username);
        }

        [HttpPost("/register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            var existingUser = await _apiContext.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser != null)
            {
                return BadRequest("Username is already taken");
            }
            if (user.Username == "" || user.Password == ""){
                return BadRequest("Both Username and password cannot be empty");

            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;
            _apiContext.Users.Add(user);
            await _apiContext.SaveChangesAsync();
            string url = "/api/users/" + user.Id;
             return CreatedAtAction(null, null, user, "The resource has been created successfully at "+ url);
        }    

        [HttpPost("/login")]
        public async Task<ActionResult<User>> Login(User user)
        {
            var userFromDb = await _apiContext.Users.SingleOrDefaultAsync(u => u.Username == user.Username);
            if (userFromDb == null || !BCrypt.Net.BCrypt.Verify(user.Password, userFromDb.Password))
            {
                return BadRequest("Username or password is incorrect");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenInformation = JwtHelper.GenerateToken(userFromDb.Id, _jwtOptions.Secret);
            if (tokenInformation["token"] == null){
                return BadRequest("No token");
            }
            var userId = JwtHelper.ReturnUserFromToken(tokenInformation["token"].ToString(), _jwtOptions.Secret);
            return Ok(new { Token = tokenInformation["token"], Id= userId });
        }

        [HttpPost("/reserve")]
        public async Task<ActionResult<Reservation>> Reserve(Reservation reservation)
        {
            _apiContext.Reservations.Add(reservation);
            await _apiContext.SaveChangesAsync();
            string url = "/reservations/" + reservation.Id;
            Console.WriteLine(reservation.UserId);
            return Ok( reservation);
        }
    }
}
