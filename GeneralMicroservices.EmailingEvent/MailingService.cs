using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralMicroservices.EmailingEvent
{
    public class MailService : IMailService
    {

        public async Task<bool> SendEmail(string recepient, string message, string msgSubject)
        {
            try
            {
                var senderEmail = "";
                var apiKey = "";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(senderEmail, "Fidarr");
                var subject = msgSubject;
                var to = new EmailAddress(recepient);
                var plainTextContent = message;
                var htmlContent = message;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
                if (response.StatusCode.ToString().ToLower() == "accepted")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }

            return false;

        }
    }
}
