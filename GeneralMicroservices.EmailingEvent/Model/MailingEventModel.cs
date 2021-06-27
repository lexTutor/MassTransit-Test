using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralMicroservices.EmailingEvent.Model
{
    public class MailingEventModel
    {
        public List<string> EmailAddresses { get; set; }
        public string Mail { get; set; }
    }
}
