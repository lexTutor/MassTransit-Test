using GeneralMicroservices.EmailingEvent.Model;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace GeneralMicroservices.EmailingEvent.Controllers.EventingControllers
{
    public class MailEventController : IConsumer<MailingEventModel>
    {
        public async Task Consume(ConsumeContext<MailingEventModel> context)
        {
             await Task.Run(() =>
             {
                  MailSender.SendMailTest();
                  //Console.WriteLine(context.Message.Mail, context.Message.EmailAddresses[0]);
             });
        }

    }
}