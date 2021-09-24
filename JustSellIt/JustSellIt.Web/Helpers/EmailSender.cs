using JustSellIt.Application;
using System.Net.Mail;

namespace JustSellIt.Web.Helpers
{
    public class EmailSender
    {
        public static void SendEmail(string link, string emailAdress, string username, string emailBody)
        {
            MailMessage mailMessage = new MailMessage("just.sell.it.contact@gmail.com", emailAdress);
            mailMessage.Body = emailBody
                .Replace("{{link}}", link)
                .Replace("{{user}}", username);

            mailMessage.Subject = "Potwierdzenie konta na portalu JustSellIt";
            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress("just.sell.it.contact@gmail.com", "JustSellIt");
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;


            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "just.sell.it.contact@gmail.com",
                Password = SystemConfiguration.Password
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }
    }
}
