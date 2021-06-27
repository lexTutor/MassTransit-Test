using GeneralMicroservices.EmailingEvent.Model;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace GeneralMicroservices.EmailingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailEventPublisherController : ControllerBase
    {
        private readonly IPublishEndpoint _publisher;
        public MailEventPublisherController(IServiceProvider provider)
        {
            _publisher = provider.GetRequiredService<IPublishEndpoint>();
        }

        [HttpPost]
        public async Task PostMail(MailingEventModel model)
        {
            await _publisher.Publish(model);
        }
    }
}
