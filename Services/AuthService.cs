using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MonthlyDataApi.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(string username, string role)
        {
            if (!UserStorage.Users.ContainsKey(username))
            {
                throw new UnauthorizedAccessException("User not found");
            }

            //var (password, role) = UserStorage.Users[username]; // מקבל את התפקיד של המשתמש

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-long-secret-key-32-bytes-long!your-very-long-secret-key-32-bytes-long!"));
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role) // מוסיף את התפקיד לטוקן
            };
            // הגדרת פרמטרים לטוקן
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
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
