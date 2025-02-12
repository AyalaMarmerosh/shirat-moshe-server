using Microsoft.Extensions.Configuration;

namespace MonthlyDataApi.Services
{
    public class LoginService
    {
        private readonly AuthService _authService;
        private readonly EmailService _emailService;


        public LoginService(IConfiguration configuration)
        {
            _authService = new AuthService(configuration);
            _emailService = new EmailService(configuration); // אתחול השירות
        }

        private static Dictionary<string, string> _verificationCodes = new();

        public string Login(string username, string password)
        {
            if (UserStorage.Users.ContainsKey(username) && UserStorage.Users[username].Password == password)
            {
                var role = UserStorage.Users[username].Role;
                return _authService.GenerateToken(username, role);
            }
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        // שלב 1 - שליחת קוד אימות למייל
        public void SendVerificationCode(string oldUsername)
        {
            if (!UserStorage.Users.ContainsKey(oldUsername))
            {
                throw new KeyNotFoundException("User not found.");
            }

            var email = GetEmailByUsername(oldUsername); // להניח שזו פונקציה שמחזירה את האימייל של המשתמש
            var code = new Random().Next(100000, 999999).ToString();

            _verificationCodes[email] = code;

            // שליחת הקוד במייל
            _emailService.SendEmail(email, "Verification Code", $"Your verification code is: {code}");
        }

        // שלב 2 - בדיקת הקוד ועדכון הפרטים
        public void UpdateCredentialsWithCode(string oldUsername, string newUsername, string newPassword, string code)
        {
            var email = GetEmailByUsername(oldUsername); // קבלת המייל של המשתמש לפי שם המשתמש הישן
            if (!_verificationCodes.ContainsKey(email) || _verificationCodes[email] != code)
            {
                throw new UnauthorizedAccessException("Invalid verification code.");
            }

            if (!UserStorage.Users.ContainsKey(oldUsername))
            {
                throw new KeyNotFoundException("User not found.");
            }

            // שמירת תפקיד המשתמש הישן
            var userRole = UserStorage.Users[oldUsername].Role;

            // עדכון שם המשתמש והססמה
            UserStorage.Users.Remove(oldUsername);
            UserStorage.Users[newUsername] = (newPassword, userRole);

            _verificationCodes.Remove(email); // מחיקת הקוד אחרי השימוש
        }

        private string GetEmailByUsername(string username)
        {
            // זו דוגמה. תוכל להחזיר את האימייל מתוך מאגר נתונים או כל מקור אחר.
            return "hyylhyyly@gmail.com";
        }
    }
}
