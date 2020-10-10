using pd_api.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(SendEmailModel model);
    }
}
