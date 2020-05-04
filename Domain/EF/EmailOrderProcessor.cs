using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace Domain.EF
{

    public class EmailOrderProcessor : IOrderProcessor
    {
        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                // наш email с заголовком письма
                MailAddress from = new MailAddress("lakshmi.sergey@gmail.com", "Adrenalin - Shop");
                // кому отправляем
                MailAddress to = new MailAddress("peskov_sergei@list.ru");
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Новый заказ!";

                StringBuilder body = new StringBuilder()
                .AppendLine("Новый заказ обработан")
                .AppendLine("-----------------")
                .AppendLine("Товары:");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (итого: {2} Тг)\n", line.Quantity, line.Product.Name, subtotal);
                }
                body.AppendFormat("\nОбщая стоимость: {0} Тг", cart.ComputeTotalValue())
                    .AppendLine("\n-----------------")
                    .AppendLine("Доставка:")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2 ?? "")
                    .AppendLine(shippingDetails.Line3 ?? "")
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.Country)
                    .AppendLine("\n" +shippingDetails.PhoneNuber)
                    .AppendLine("-----------------")
                    .AppendFormat("Подарочная упаковка: {0}", shippingDetails.GiftWrap ? "Да" : "Нет");

                m.Body = body.ToString();
                SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                // логин и пароль
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential("lakshmi.sergey@gmail.com", "veryhardpassword228");
                smtp.Send(m);
            }
        }
    }
}
