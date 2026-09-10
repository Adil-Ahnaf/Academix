using System.Net.Mail;
using System.Net;

namespace Portal.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        string host, username, password, senderEmail, senderName;
        int port;
        public EmailHelper(IConfiguration configuration)
        {
            host = configuration.GetValue<string>("SMTP:Host");
            port = int.Parse(configuration.GetValue<string>("SMTP:Port"));
            username = configuration.GetValue<string>("SMTP:Username");
            password = configuration.GetValue<string>("SMTP:Password");
            senderEmail = configuration.GetValue<string>("SMTP:User");
            senderName = configuration.GetValue<string>("EmailDisplayName");
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var message = new MailMessage(senderEmail, to, subject, body);
            message.From = new MailAddress(senderEmail, senderName);
            message.IsBodyHtml = true;
            using (var emailClient = new SmtpClient(host, port))
            {
                emailClient.EnableSsl = true;
                emailClient.Credentials = new NetworkCredential(username, password);
                await emailClient.SendMailAsync(message);
            }
        }

        public async Task SendBulkEmailAsync(List<string> toemails, string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.Subject = subject;
            message.Body = body;
            message.From = new MailAddress(senderEmail, senderName);
            message.IsBodyHtml = true;
            message.To.Add(message.From);

            foreach (var emailAdress in toemails)
            {
                message.Bcc.Add(emailAdress);
            }

            using (var emailClient = new SmtpClient(host, port))
            {
                emailClient.EnableSsl = true;
                emailClient.Credentials = new NetworkCredential(username, password);
                await emailClient.SendMailAsync(message);
            }
        }
    }
}
