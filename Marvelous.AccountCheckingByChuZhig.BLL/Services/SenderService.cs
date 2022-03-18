using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class SenderService
    {
        public void SendMail(MessageModel messageModel)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("golenkotoxa@gmail.com", "Tom");
            // кому отправляем
            MailAddress to = new MailAddress("goltoha@mail.ru");
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Тест";
            // текст письма
            m.Body = "<h2>Письмо-тест работы smtp-клиента</h2>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("golenkotoxa@gmail.com", "golenkotoxa");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}
