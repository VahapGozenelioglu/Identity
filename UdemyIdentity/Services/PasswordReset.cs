using System.Net;
using System.Net.Mail;

namespace UdemyIdentity.Services
{
    public static class PasswordReset
    {
        public static void PasswordResetSendEmail(string link)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
            };

            smtpClient.Credentials = new NetworkCredential("gozeneliogluvahap@gmail.com", "ufgzrtgzvqsggzby");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("gozeneliogluvahap@gmail.com");
            mailMessage.To.Add("murphyslaws1@outlook.com");
            mailMessage.Subject = "--Password Reset--";
            mailMessage.Body = "<h2> Please click the link below to reset your password </h2> <hr/>";
            mailMessage.Body += $"<a href='{link}'> Password reset link <a/>";
            mailMessage.IsBodyHtml = true;

            smtpClient.Send(mailMessage);
        }

    }
}
