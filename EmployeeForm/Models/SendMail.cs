using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace EmployeeForm.Models
{
    public static class SendMail
    {
        public static bool  SendMails(string to)
        {

            try

            {
                string GmailEmailID = ConfigurationManager.AppSettings["GmailEmailID"];
                string GmailPassward = ConfigurationManager.AppSettings["GmailPassward"];

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(GmailEmailID);

                mail.To.Add(to);

                mail.Subject = "Employee Registration";

                mail.Body = "Congratulations you are registered successfully!";

                SmtpServer.Port = 587;

                SmtpServer.Credentials = new System.Net.NetworkCredential(GmailEmailID, GmailPassward);

                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}