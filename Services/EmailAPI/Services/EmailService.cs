using QuickFix.Services.EmailAPI.Data;
using QuickFix.Services.EmailAPI.Models;
using Infrastructure.Email;


namespace QuickFix.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly AppDbContext _db;
        private readonly IEmailSender _emailSender;

        public EmailService(AppDbContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }


        public async Task RegisterUserEmailAndLog(string receiverEmailAddress)
        {
            string message = "User Registeration Successful. <br/> Welcome! : " + receiverEmailAddress;
            await LogAndEmail(message, receiverEmailAddress);
        }

        private async Task<bool> LogAndEmail(string message, string receiverEmailAddress)
        {
            try
            {
                EmailLog emailLog = new()
                {
                    Sender = "",
                    Receiver = receiverEmailAddress,
                    EmailSent = DateTime.Now,
                    Message = message
                };

                //save to db
                await _db.EmailLogs.AddAsync(emailLog);
                await _db.SaveChangesAsync();

                //send email
                await _emailSender.SendEmailAsync(receiverEmailAddress, "Welcome", message);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
