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
        public static void SuiteStatus(SuiteModel suite)
        {
            string user = "";
            string pw = "";


            string to = "joakim.odermalm@unicus.no";
            string from = "donotreply@gmail.com";
            string subject = "TestRunnerApp suite status " + DateTime.Now.ToString();

            string body = "<html><body><pre>" + Environment.NewLine;
            body += SuiteToTable(suite);
            body += Environment.NewLine + "</pre></body></html>"; 


            Send(to, from, subject, body, user, pw);

        }


        public static void Send(string to, string from, string subject, string body, string user, string pw)
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

        static string SuiteToTable(SuiteModel suite)
        {
            if (suite.tests.Count() < 1)
            {
                return "No tests found.";
            }

            string[] columns = { "ID", "Name", "Last run", "Last outcome", "Ex message" };

            var table = suite.tests.ToStringTable(columns,
                     t => t.id,
                     t => t.name,
                     t => t.previousDateTime == null ? string.Empty : t.previousDateTime.ToString(),
                     t => t.previousOutcome,
                     t => t.runs.Count() > 0 ? t.runs.Last()?.resultObj?.eMessage : string.Empty


                 );

            return table;

        }

    }
}
