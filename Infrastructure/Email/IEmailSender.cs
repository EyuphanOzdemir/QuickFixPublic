using System.Threading.Tasks;

namespace Infrastructure.Email
{
  public interface IEmailSender
  {
        Task SendEmailAsync(string to, string subject, string body);
  }
}
