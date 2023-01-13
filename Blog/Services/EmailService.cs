using System.Runtime.CompilerServices;
using System.Net;
using System.Net.Mail;
namespace Blog.Services
{
    public class EmailService
    {
        public bool Send
        (
            string toName,
            string toEmail,
            string subject,
            string body,
            string fromName = "Equipe de desenvolvimento Blog",
            string fromEmail = "nevesbruno814@gmail.com"
        )
        {
            var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);

            smtpClient.Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = false;

            var email = new MailMessage();

            email.From = new MailAddress(fromEmail, fromName);
            email.To.Add(new MailAddress(toEmail, toName));
            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;


            try
            {
                smtpClient.Send(email);
                return true;
            }
            catch (System.Exception ex)
            {
               Console.WriteLine(ex) ;
               return false;
            }
        }
    }
}