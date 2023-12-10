using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.Email
{
    public interface IEmailService
    {
        bool SendEmail(string receiver_email, string body);
    }
}
