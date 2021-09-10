using System;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HotelReservationAPI.Helper.MailService
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        public EmailSender (IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;

                var builder = new BodyBuilder();
                if (mailRequest.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in mailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }

                builder.HtmlBody = mailRequest.Body;
                email.Body = builder.ToMessageBody();

                using SmtpClient smtp = new SmtpClient();

                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                

        }

        public async Task SendEmailEasyAsync(MailRequest mailRequest)
        {
            MimeMessage message = new MimeMessage();
            var builder = new BodyBuilder();

            message.From.Add(new MailboxAddress("Peter Adebimpe",_mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            message.Subject = mailRequest.Subject;
            builder.HtmlBody = mailRequest.Body;
            message.Body = builder.ToMessageBody();

            SmtpClient smtp = new SmtpClient();
            try
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port);
                smtp.Authenticate(_mailSettings.Mail,_mailSettings.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                smtp.Disconnect(true);
                smtp.Dispose();
            }

        }

    }
}
