using Serilog;
using System;
using System.Net.Mail;
using WebApi.Helpers.Common;
using WebApi.Models;

namespace WebApi.Helpers.Services
{
    public class EmailService
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(EmailService));
        public static bool SendMail(string to, string cc, string subject, string body, string attachment, string attachmentFileName, string sPasswordAttach, ref TransactionReponse reponse)
        {
            SmtpClient smtpClient = GlobalVariable.SmtpClient;
            MailMessage message = new MailMessage
            {
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.Delay
            };
            if (!string.IsNullOrEmpty(GlobalVariable.EmailDelivery))
            {
                message.Headers.Add("Disposition-Notification-To", GlobalVariable.EmailDelivery);
            }

            message.From = new MailAddress(GlobalVariable.EmailFrom);
            message.To.Add(to);
            if (!string.IsNullOrEmpty(cc))
                message.CC.Add(cc);

            message.Subject = subject;
            message.IsBodyHtml = true;

            // Message body content
            message.Body = body;
            try
            {
                smtpClient.SendAsync(message, null);
            }
            catch (SmtpFailedRecipientsException ex)
            {
                _logger.Error(ex.ToString());
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        System.Threading.Thread.Sleep(3000);
                        smtpClient.SendAsync(message, null);
                    }
                    else
                    {
                        throw new Exception(string.Format("Failed to deliver message to {0}", ex.InnerExceptions[i].FailedRecipient));
                    }
                }
            }
            return true;
        }
    }
}
