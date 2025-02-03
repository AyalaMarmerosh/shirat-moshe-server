namespace MonthlyDataApi.Services
{
    public class LoginService
    {
        private readonly AuthService _authService;
        private static Dictionary<string, string> _users = new()
    {
        { "שירת משה", "אבא שלי" } 
    };

        public LoginService()
        {
            _authService = new AuthService();
        }

        public string Login(string username, string password)
        {
            if (_users.ContainsKey(username) && _users[username] == password)
            {
                return _authService.GenerateToken(username);
            }
            throw new UnauthorizedAccessException("Invalid credentials.");
        }
        public void UpdateCredentials(string oldUsername, string newUsername, string newPassword)
        {
            if (!_users.ContainsKey(oldUsername))
            {
                throw new KeyNotFoundException("User not found.");
            }

            // עדכון פרטי המשתמש
            _users.Remove(oldUsername);
            _users[newUsername] = newPassword;
        }
    }
}
