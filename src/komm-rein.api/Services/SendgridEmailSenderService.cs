using komm_rein.api.Config;
using komm_rein.model;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Services
{
    public class SendgridEmailSenderService : IEmailSenderService
    {

        private readonly SendGridOptions _options;

        public SendgridEmailSenderService(IOptions<SendGridOptions> options)
        {
            _options = options.Value;
        }

        public Task Send(string subject, string message, string emailAddress)
        {
            var client = new SendGridClient(_options.ApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_options.From, _options.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(emailAddress));

            msg.SetClickTracking(_options.EnableClickTracking, _options.EnableClickTrackingText);

            return client.SendEmailAsync(msg);
        }

        public Task SendVisit(Visit visit, string email)
        {
            if(visit == null)
            {
                throw new ArgumentException("Param null", "visit");
            }

            if (visit.ID == Guid.Empty)
            {
                throw new ArgumentException("visit id is empty");
            }

            StringBuilder message = new StringBuilder();
            message.Append($"Dein Termin: https://terminshopping.app/mein-termin/{visit.ID}");

            return Send("Dein Termin", message.ToString(), email);
        }
    }
}
