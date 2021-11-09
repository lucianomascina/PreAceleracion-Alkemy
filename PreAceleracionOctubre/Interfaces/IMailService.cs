using PreAceleracionOctubre.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.Interfaces
{
    public interface IMailService
    {
        Task SendEmail(User user);
    }
}
