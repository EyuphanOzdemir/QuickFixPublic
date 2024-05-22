namespace QuickFix.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task RegisterUserEmailAndLog(string email);
    }
}
