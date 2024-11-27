using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using backend.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;


using backend.Helpers;

namespace backend.Controllers
{
    [Route("[controller]")]
    public class Reservations : ControllerBase
    {
        private ApiContext _apiContext;
        private JwtOptions _jwtOptions;


        public Reservations(ApiContext context, IOptions<JwtOptions> jwtOptions)
        {
            _apiContext = context;
            _jwtOptions = jwtOptions.Value;

        }

        [HttpGet("/getAllReservations/{token}")]
        public ActionResult<IEnumerable<Reservation>> GetReservationsOfLoggedInUser(string token)
        {
            var validatedToken = JwtHelper.ValidateJwt(token, _jwtOptions.Secret);
            if (validatedToken == null)
            {
                return Unauthorized();

            }
            var userId = JwtHelper.ReturnUserFromToken(token, _jwtOptions.Secret);

            var reservations = _apiContext.Reservations.Where(r => r.UserId == userId).ToList();
            return Ok(reservations);
        }
        [HttpGet("/getAvailableRooms/{checkin}to{checkout}")]
        public ActionResult<IEnumerable<Room>> GetAvailableRooms(DateTime checkin, DateTime checkout)
        {
            if (checkin == checkout  || checkin <= DateTime.Now.AddDays(-1) || checkin >= checkout || checkout < DateTime.Now){
                return BadRequest("Invalid dates, please select the correct date pattern. Make sure Check-in or check-out dates are not from the the past and check in is not greater than or equal to checkout date. ");
            }

            var unavailableRoomIds = _apiContext.Reservations
                .Where(r => r.CheckInDate < checkout && r.CheckOutDate > checkin)
                .Select(r => r.RoomId)
                .Distinct();


            var availableRooms = _apiContext.Rooms
                .Where(room => !unavailableRoomIds.Contains(room.Id))
                .ToList();
    
            return availableRooms;
        }
    }

}