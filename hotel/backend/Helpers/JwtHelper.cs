using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Helpers
{
    public class JwtHelper
    {
        public static Dictionary<string, object> GenerateToken(int id, string jwtSecretKey){
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(jwtSecretKey);

            if (key.Length < 32) // 256 bits
            {
                Array.Resize(ref key, 32);
            }           
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userId", id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(50),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var expiration = token.ValidTo;
            var tokenString = tokenHandler.WriteToken(token);
            var tokenData = new Dictionary<string, object>
            {
                { "token", tokenString },
                { "expiration", expiration }
            };

            return tokenData;
        }

        public static ClaimsPrincipal ValidateJwt(string tokenString, string jwtSecretKey)
        {
            try
            {
                // Create a JwtSecurityTokenHandler
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(jwtSecretKey);         
                // Create a TokenValidationParameters object
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                // Validate the token
                var validatedToken = tokenHandler.ValidateToken(tokenString, validationParameters, out var extractedToken);
                var currentUtc = DateTime.UtcNow;
                if (extractedToken.ValidTo < currentUtc)
                {
                    // Token has expired
                    return new ClaimsPrincipal(new ClaimsIdentity());
                }

                // Return the validated token
                return validatedToken;
            }
            catch (SecurityTokenValidationException ex)
            {
                // Token validation failed due to a security-related issue
                Console.WriteLine($"Token validation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Other exceptions occurred during token validation
                Console.WriteLine($"An error occurred during token validation: {ex.Message}");
            }
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        public static int ReturnUserFromToken(string token, string jwtSecretKey){
            
            var validatedToken = JwtHelper.ValidateJwt(token, jwtSecretKey);

            var userIdClaim = validatedToken.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return -1;
            }

            // Parse the userId value to an integer
            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                // userId successfully parsed, return the value
                return userId;
            }

            return userId;
        }

    }
}