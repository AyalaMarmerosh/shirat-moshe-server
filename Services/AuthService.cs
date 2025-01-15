using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MonthlyDataApi.Services
{
    public class AuthService
    {
        public string GenerateToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-long-secret-key-32-bytes-long!your-very-long-secret-key-32-bytes-long!"));
            var issuer = Environment.GetEnvironmentVariable("ISSUER") ?? "http://localhost";
            var audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? "http://localhost:4200";

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // הגדרת פרמטרים לטוקן
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                // פרטי המשתמש – ניתן להוסיף יותר נתונים (Claim)
                new Claim(ClaimTypes.Name, username)
            }),
                Expires = DateTime.Now.AddHours(1),  // הזמן שבו הטוקן יפוג
                SigningCredentials = credentials,
                Issuer = issuer,   
                Audience = audience 
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);  // יצירת הטוקן
            return tokenHandler.WriteToken(token);  // החזרת הטוקן
        }
    }
}
