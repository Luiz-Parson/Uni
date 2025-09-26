using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System;
using ConnectorAccess.Service.models.dtos;

namespace ConnectorAccess.Service.Services
{
    public class EmailService
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUser;
        private readonly string smtpPass;
        private readonly int hour;
        private readonly int minute;
        private readonly List<string> recipients;
        private readonly AccessControlDay accessControlDay;

        public EmailService(IConfiguration configuration, AccessControlDay accessControlDay)
        {
            var emailSettings = configuration.GetSection("EmailSettings");
            var schedule = emailSettings.GetSection("Schedule");
            smtpServer = emailSettings["SmtpServer"];
            smtpPort = int.Parse(emailSettings["SmtpPort"]);
            smtpUser = emailSettings["SmtpUser"];
            smtpPass = emailSettings["SmtpPass"];
            recipients = emailSettings.GetSection("Recipients").Get<List<string>>() ?? new List<string>();
            hour = schedule.GetValue<int>("Hour", 12);
            minute = schedule.GetValue<int>("Minute", 0);
            this.accessControlDay = accessControlDay;
        }

        public async Task SendEmailAsync(List<string> recipients, string subject, string body, string attachmentPath)
        {
            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUser),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                foreach (var recipient in recipients)
                {
                    mailMessage.To.Add(recipient);
                }

                if (File.Exists(attachmentPath))
                {
                    mailMessage.Attachments.Add(new Attachment(attachmentPath));
                }

                await client.SendMailAsync(mailMessage);
            }
        }

        public (int Hour, int Minute) GetScheduledTime()
        {
            return (hour, minute);
        }
    }
}
