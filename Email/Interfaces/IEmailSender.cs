using Email.Models;
using System.Threading.Tasks;

namespace Email.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}
