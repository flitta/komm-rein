using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Services
{
    public interface IEmailSenderService
    {
        public Task SendVisit(Visit visit, string email);

        Task Send(string subject, string message, string emailAddress);
    }
}
