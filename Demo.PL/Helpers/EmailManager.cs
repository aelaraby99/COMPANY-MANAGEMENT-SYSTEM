using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
    public class EmailManager
    {
        public static void SendEmail(string subject, string to, string body)
        {
            var email = CreateEmail(subject, to, body);
            SMTPEmail(email);
           
        }
        private static Email CreateEmail(string subject, string to, string body)
        {
            var email = new Email()
            {
                Subject = subject,
                To = to,
                Body = body,
            };
            return email;
        }

        private static void SMTPEmail(Email email)
        {
            var Client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("ahmedtst61@gmail.com", "oznsnmpzaqmtiupa")
            };

            Client.Send("ahmedtst61@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
