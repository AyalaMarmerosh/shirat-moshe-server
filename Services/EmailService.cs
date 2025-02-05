using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MonthlyDataApi.Services
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;

        // קונסטרוקטור שיקבל את ההגדרות מהקונפיגורציה
        public EmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["EmailSettings:SmtpServer"];
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
            _smtpUser = configuration["EmailSettings:SmtpUser"];
            _smtpPassword = configuration["EmailSettings:SmtpPassword"];
        }

        public async Task SendEmail(string toEmail, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Your App Name", _smtpUser)); // המייל שלך (שם תצוגה+מייל)
            emailMessage.To.Add(new MailboxAddress("", toEmail)); // הכתובת שאליה שולחים את המייל
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message }; // תוכן ההודעה

            var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls); // חיבור לשרת
            await client.AuthenticateAsync(_smtpUser, _smtpPassword);  // אתחול עם המייל שלך וסיסמת האפליקציה
            await client.SendAsync(emailMessage);  // שליחת המייל
            await client.DisconnectAsync(true);   // ניתוק מהשרת
        }
    }
}
