using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Email
{
  public class EmailSender(EmailConfig config) : IEmailSender
  {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Create an instance of the SmtpClient
            using var smtpClient = new SmtpClient(config.SMTPServer, config.SMTPPort);
            smtpClient.Credentials = new NetworkCredential(config.UserName, config.Password);
            smtpClient.EnableSsl = config.EnableSSL;
            // Create a MailMessage object
            using (var mailMessage = new MailMessage())
            {
                // Set the sender email address
                mailMessage.From = new MailAddress(config.UserName);

                // Set the recipient email address
                mailMessage.To.Add(new MailAddress(to));

                // Set the subject and body of the email
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                // Send the email
                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

	}

}

