namespace MonthlyDataApi.Services
{
    public static class UserStorage
    {
        public static readonly Dictionary<string, (string Password, string Role)> Users = new()
        {
            { "שירת משה", ("מנהל", "Admin") },
            { "כולל שירת משה", ("משרד", "Viewer") }
        };
    }
}
