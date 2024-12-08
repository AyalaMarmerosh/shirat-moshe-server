namespace MonthlyDataApi.Services
{
    public class LoginService
    {
        private readonly AuthService _authService;

        public LoginService()
        {
            _authService = new AuthService();
        }

        public string Login(string username, string password)
        {
            // כאן תוסיף את ההיגיון שלך לבדוק אם שם המשתמש והסיסמא נכונים.
            if (username == "שירת משה" && password == "אבא שלי")
            {
                // אם ההתחברות הצליחה, יצור טוקן
                return _authService.GenerateToken(username);
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }
        }
    }
}
