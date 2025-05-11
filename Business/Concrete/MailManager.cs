using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Configuration;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using Utilitys.ExceptionHandler;
using Utilitys.ResponseHandler;

namespace Utilitys.MailServices
{
    public class MailManager : Business.Abstract.IMailServices
    {
        private readonly EmailSender _mail;
        private readonly UserManager<User> _userManager;
        public MailManager(IOptions<EmailSender> mail, UserManager<User> userManager) 
        {
            _mail = mail.Value;
            _userManager = userManager;
        }

        public async Task<ResponseModel> SendingEmail(string email, string url)    //callback url required
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Email Verification<No-Reply>", _mail.Email));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "Email Verification by SelfBookAPI";

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<b>Email verification url: </b>" + "<a href = " + url + "> link text </a> <br>" +
                    "<br> <p>If link doesn't work : " + url + "</p>";

                message.Body = bodyBuilder.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                    client.Authenticate(_mail.Email, _mail.Password);
                    client.Send(message);
                    client.Disconnect(true);
                }
                return null;
            }
            catch (Exception ex)
            {
                return new ResponseModel { Message = "Exception throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException.Message) };
            }
        }
        public async Task<ResponseModel> ConfirmEmail(string userid, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userid);
                if (user == null)
                    return new ResponseModel { Message = "User not found!!" };

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (!result.Succeeded)
                    return new ResponseModel { Message = "Email confirmation failed!" } ;
                return null;
            }
            catch(Exception ex)
            {
                return new ResponseModel { Message = "Exception throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException.Message) };
            }

        }

    }
}
