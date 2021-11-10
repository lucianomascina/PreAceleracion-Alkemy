using PreAceleracionOctubre.Entities;
using PreAceleracionOctubre.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.Services
{
    public class MailService : IMailService
    {
        private readonly ISendGridClient _sendGridClient;

        public MailService(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }
        public async Task SendEmail(User user)
        {
            try
            {
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress("lucianomascina@gmail.com", "API de Disney"),
                    Subject = "se ha registrado satisfactoriamente!",
                    PlainTextContent = $"se ha creado el usuario con nombre {user.UserName} de manera correcta."
                };

                msg.AddTo(new EmailAddress(user.Email, "test User"));

                await _sendGridClient.SendEmailAsync(msg);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
