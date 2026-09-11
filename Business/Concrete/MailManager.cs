using Business.Abstract;
using Entities;
using Entities.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using Utilitys.Logging.ExceptionHandler;

namespace Utilitys.MailServices
{
    public class MailManager : IMailServices
    {
        private readonly EmailSender _mail;
        private readonly UserManager<User> _userManager;
        public MailManager(IOptions<EmailSender> mail, UserManager<User> userManager)
        {
            _mail = mail.Value;
            _userManager = userManager;
        }

        public async Task SendingEmail(string email, string url)    //callback url required
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Email Verification<No-Reply>", _mail.Email));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Email Verification by SkyAirportAPI";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<b>Email verification url: </b>" + "<a href = " + url + "> link text </a> <br>" +
                "<br> <p>If link doesn't work : " + url + "</p>";

            message.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate(_mail.Email, _mail.Password);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
        public async Task ConfirmEmail(string userid, string token)
        {
            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(token))
                throw new CustomException("Id or Token is empty!!", (int)HttpStatusCode.BadRequest);
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
                throw new CustomException("User not found!!", (int)HttpStatusCode.NotFound);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                throw new CustomException("Email confirmation failed!", (int)HttpStatusCode.NotFound, result.Errors?.FirstOrDefault()?.ToString());
        }

    }
}
