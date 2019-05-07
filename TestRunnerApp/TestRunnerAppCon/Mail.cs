using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRunnerLib;
using System.Net.Mail;

namespace TestRunnerAppCon
{

    class Mail
    {
        public static void SuiteStatus(SuiteModel suite, string sendTo, string[] filters)
        {
            //Outcome[] filter = { Outcome.Fail, Outcome.Warning };
            
            Col[] selection = { Col.id, Col.name, Col.previousDateTime, Col.webDriverType, Col.previousOutCome,
                                Col.failStep, Col.message, Col.eType};

            string user = "user@gmail.com";
            string pw = "password";

            //string to = "joakim.odermalm@unicus.no";
            string to = sendTo;
            string from = "donotreply@gmail.com";
            string subject = "TestRunnerApp suite status " + DateTime.Now.ToString();

            string body = @"<html><body>";
            body += @"<style> pre { font-family: 'Consolas', 'Lucida Console', 'Courier New', monospace; } </style>";
            body += @"<style> p { font-family: 'Consolas', 'Lucida Console', 'Courier New', monospace; } </style>";
            body += Environment.NewLine;
            body += @"<p>" + Report.SuiteToTable(suite, true, Report.readFilters(filters), selection) + "</p>";
            //body += @"<pre>" + SuiteToTable(suite) + "</pre>";
            body += Environment.NewLine;
            body += @"</body></html>"; 


            SendGmail(to, from, subject, body, user, pw);

        }


        public static void SendGmail(string to, string from, string subject, string body, string user, string pw)
        {

            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(user, pw);



            MailMessage mm = new MailMessage(from, to, subject, body);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.IsBodyHtml = true;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


            client.Send(mm);
            Console.WriteLine("Email sent.");
        }

        public static void SendLocal(string to, string from, string subject, string body, string user, string pw)
        {

            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.Host = "192.168.0.112";
            client.EnableSsl = false;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(user, pw);



            MailMessage mm = new MailMessage(from, to, subject, body);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.IsBodyHtml = true;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


            client.Send(mm);
            Console.WriteLine("Email sent.");
        }


        
    }
}
