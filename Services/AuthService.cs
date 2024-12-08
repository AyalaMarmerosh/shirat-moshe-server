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
            // מפתח סודי, השתדל לשמור אותו במקום מאובטח!
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-long-secret-key-32-bytes-long!your-very-long-secret-key-32-bytes-long!"));

            // הגדרת החתימה של הטוקן (SigningCredentials)
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
                Issuer = "your-issuer",   // יש להחליף בשם הארגון או האתר שלך
                Audience = "your-audience" // יש להחליף ב-audience המתאים
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);  // יצירת הטוקן
            return tokenHandler.WriteToken(token);  // החזרת הטוקן
        }
    }
}
