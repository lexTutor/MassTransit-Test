using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralMicroservices.EmailingEvent
{
    public interface IMailService
    {
        Task<bool> SendEmail(string recepient, string message, string msgSubject);
    }
}
