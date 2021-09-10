using System.Threading.Tasks;

namespace HotelReservationAPI.Helper.MailService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendEmailEasyAsync(MailRequest mailRequest);

    }
}
