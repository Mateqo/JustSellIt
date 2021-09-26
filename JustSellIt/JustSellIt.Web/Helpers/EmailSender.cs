using JustSellIt.Application;
using JustSellIt.Application.ViewModels.Base;
using System.Net.Mail;

namespace JustSellIt.Web.Helpers
{
    public class EmailSender
    {
        public static void SendEmail(string link, string emailAdress, string username, EmailType type)
        {
            string emailConfirmBody = "<!DOCTYPE html><html><head><title></title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"><meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><style type=\"text/css\">body{background-color:#f2f4f5;margin:0;padding:0;font-family:Lato,Helvetica,Arial,sans-serif}.card{background-color:#fff;min-height:740px;margin:auto;width:50%;margin-top:-150px;text-align:center;border-radius:4px}@media only screen and (max-width:900px){.card{width:100%;margin-left:0;margin-right:0;}}.title{padding-top:60px;font-size:40px;font-weight:400}.fa-handshake{padding-top:20px;font-size:70px;color:#36486b}.text{display:block;text-align:left;padding-left:40px;padding-right:40px;font-size:18px;color:#666;line-height:27px}.activeButton{color:white;text-decoration:none;margin-top:25px;padding:15px 25px;color:#fff;background-color:#36486b;border:none;border-radius:2px;font-size:20px}.activeButton:hover{cursor:pointer}.link{color:#36486b;text-decoration:underline}img{height:140px;width:140px;border-radius:50%;margin-top:10px}</style></head><div class=\"card\"><h1 class=\"title\">Witaj {{user}}!</h1><img src=\"https://cdn.pixabay.com/photo/2017/03/21/01/59/business-2160910_960_720.png\" alt=\"icon\"><p class=\"text\" style=\"margin-bottom:40px;padding-top:20px\">Cieszymy się, że zarejestrowałeś się na naszym portalu JustSellIt. Najpierw musisz potwierdzić swoje konto. Wystarczy nacisnąć przycisk poniżej.</p><a href=\"{{link}}\" class=\"activeButton\">Potwierdź konto</a><p class=\"text\" style=\"padding-top:35px\">Jeśli przycisk nie zadziała, skopiuj i wklej następujący link w przeglądarce:</p><p class=\"text link\">{{link}}</p><p class=\"text\">Jeśli masz jakieś pytania, po prostu odpowiedz na tego e-maila. Zawsze chętnie Ci pomożemy.</p><p class=\"text\">Pozdrawiamy,</p><p class=\"text\" style=\"padding-bottom:30px\">Zespół JustSellIt</p></div></body>";
            string emailForgotPasswordBody = "<!DOCTYPE html><html><head><title></title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"><meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><style type=\"text/css\">body{background-color:#f2f4f5;margin:0;padding:0;font-family:Lato,Helvetica,Arial,sans-serif}.card{background-color:#fff;min-height:740px;margin:auto;width:50%;margin-top:-150px;text-align:center;border-radius:4px}@media only screen and (max-width:900px){.card{width:100%;margin-left:0;margin-right:0;}}.title{padding-top:60px;font-size:40px;font-weight:400}.fa-handshake{padding-top:20px;font-size:70px;color:#36486b}.text{display:block;text-align:left;padding-left:40px;padding-right:40px;font-size:18px;color:#666;line-height:27px}.activeButton{color:white;text-decoration:none;margin-top:25px;padding:15px 25px;color:#fff;background-color:#36486b;border:none;border-radius:2px;font-size:20px}.activeButton:hover{cursor:pointer}.link{color:#36486b;text-decoration:underline}img{height:140px;width:140px;border-radius:50%;margin-top:10px}</style></head><div class=\"card\"><h1 class=\"title\">Witaj {{user}}!</h1><img src=\"https://cdn.pixabay.com/photo/2017/04/10/12/18/castle-2218358_960_720.jpg\" alt=\"icon\"><p class=\"text\" style=\"margin-bottom:40px;padding-top:20px\">Zauważyliśmy, że masz problem z przypomniniem hasła. Jeżeli to Ty postanowaiłeś je zresetować wystarczy nacisnąć przycisk poniżej. W przeciwnym wypadku nie korzystaj z takiej możliwości.</p><a href=\"{{link}}\" class=\"activeButton\">Restartowanie hasła</a><p class=\"text\" style=\"padding-top:35px\">Jeśli przycisk nie zadziała, skopiuj i wklej następujący link w przeglądarce:</p><p class=\"text link\">{{link}}</p><p class=\"text\">Jeśli masz jakieś pytania, po prostu odpowiedz na tego e-maila. Zawsze chętnie Ci pomożemy.</p><p class=\"text\">Pozdrawiamy,</p><p class=\"text\" style=\"padding-bottom:30px\">Zespół JustSellIt</p></div></body>";

            MailMessage mailMessage = new MailMessage("just.sell.it.contact@gmail.com", emailAdress);

            switch (type)
            {
                case EmailType.Confirmation:
                    mailMessage.Body = emailConfirmBody;
                    mailMessage.Subject = "Potwierdzenie konta JustSellIt";
                    break;
                case EmailType.ForgotPassword:
                    mailMessage.Body = emailForgotPasswordBody;
                    mailMessage.Subject = "Restartowanie hasła JustSellIt";
                    break;
                default:
                    break;
            }

            mailMessage.Body = mailMessage.Body
                .Replace("{{link}}", link)
                .Replace("{{user}}", username);

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
