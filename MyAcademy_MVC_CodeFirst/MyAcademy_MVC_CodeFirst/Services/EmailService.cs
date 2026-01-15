using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System.Configuration;

namespace MyAcademy_MVC_CodeFirst.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var fromEmail = ConfigurationManager.AppSettings["MailUser"];
            var password = ConfigurationManager.AppSettings["MailPass"];

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Lifesure Sigorta", fromEmail)); // GÖNDEREN
            mimeMessage.To.Add(new MailboxAddress("", toEmail)); // ALICI
            mimeMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
    <div style='font-family: Arial, sans-serif; padding: 20px;'>
        {body} 
        <br><br>
        <hr>
        <small>Bu mesaj Yapay Zeka asistanımız tarafından oluşturulmuştur.</small>
    </div>";
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync(fromEmail, password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
