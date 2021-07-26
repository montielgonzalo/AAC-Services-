using Email.Interfaces;
using Email.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;

        }

        public async Task SendEmailAsync(Message message)
        {
            try
            {
                var emailMessage = CreateEmailMessage(message);

                await SendAsync(emailMessage);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            try
            {
                _logger.Debug("CreateEmailMessage");
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
                emailMessage.To.AddRange(message.To);

                if (message.Cc != null && message.Cc.Any())
                {
                    emailMessage.Cc.AddRange(message.Cc);
                }

                emailMessage.Subject = message.Subject;

                var bodyBuilder = GetMailBody(message.Content);

                if (message.Attachments != null && message.Attachments.Any())
                {
                    byte[] fileBytes;
                    foreach (var attachment in message.Attachments)
                    {
                        using (var ms = new MemoryStream())
                        {
                            attachment.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }

                        bodyBuilder.Attachments.Add(
                            attachment.FileName,
                            fileBytes,
                            ContentType.Parse(attachment.ContentType));
                    }
                }

                emailMessage.Body = bodyBuilder.ToMessageBody();
                return emailMessage;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task SendAsync(MimeMessage message)
        {
            try
            {
                using (var client = new SmtpClient())
                {

                    await client.ConnectAsync(
                        _emailConfig.SmtpServer,
                        _emailConfig.Port,
                        _emailConfig.Ssl == true ? SecureSocketOptions.StartTls : SecureSocketOptions.None);


                    if (_emailConfig.UseAuthentication == true)
                    {

                        if (string.IsNullOrEmpty(_emailConfig.Domain))
                        {
                            await client.AuthenticateAsync(
                                new NetworkCredential(
                                    _emailConfig.UserName,
                                    _emailConfig.Password));
                        }
                        else
                        {
                            await client.AuthenticateAsync(
                                new NetworkCredential(
                                    _emailConfig.UserName,
                                    _emailConfig.Password,
                                    _emailConfig.Domain));
                        }
                    }
                    else
                    {
                        _logger.Debug("no authenticacion");
                    }

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public BodyBuilder GetMailBody(string message)
        {
            try
            {
                var builder = new BodyBuilder();
                builder.HtmlBody = string.Format(@"
                <body style='margin:0'>
                    <table width='100%' style='height: 100 %; font-family: Calibri' cellpadding='10'>
                        <tr>
                            <td style='background-color: #000055; text-align: center; color: white; font-size: 20px; font-weight:bold'>Automatic Aquarium Care</td>
                        </tr>
                        <tr>
                            <td align='justify'>
                                {0}
                            </td>
                        </tr>
                    </table>
                </body>", message);

                return builder;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}