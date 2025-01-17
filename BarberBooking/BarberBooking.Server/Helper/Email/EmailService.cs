
using BarberBooking.Server.Helper.Exceptions;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BarberBooking.Server.Helper.Email
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string email, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Barber Booking", "timeofftracker2024@gmail.com"));
            message.To.Add(new MailboxAddress("",email));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text= body };

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate("timeofftracker2024@gmail.com", "rlqxqowojwojhrkt");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch(EmailServiceException ex)
            {
                throw new EmailServiceException("There was an error while sending email ... Please try again.");
            }
        }
    }
}
