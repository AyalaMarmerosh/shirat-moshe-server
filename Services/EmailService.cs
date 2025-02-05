using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MonthlyDataApi.Services
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";   // שרת ה-SMTP של Gmail
        private readonly int _smtpPort = 587;                      // פורט SMTP של Gmail
        private readonly string _smtpUser = "hyylhyyly@gmail.com"; // כתובת המייל שלך ב-Gmail
        private readonly string _smtpPassword = "libw pdeh iwrj ntqx"; // סיסמת האפליקציה שלך (לא הסיסמה הרגילה)

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
