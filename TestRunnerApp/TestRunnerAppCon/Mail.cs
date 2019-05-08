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
        public static void SuiteStatus(SuiteModel suite, string sendTo, string[] filters, string idPattern)
        {
            Col[] selection = { Col.id, Col.name, Col.previousDateTime, Col.webDriverType, Col.previousOutcome,
                                Col.failStep, Col.message, Col.eType};

            if (string.IsNullOrEmpty(Settings.sendFromAccount) || 
                string.IsNullOrEmpty(Settings.sendFromAccountPw) ||
                string.IsNullOrEmpty(Settings.subject))
            {
                Console.WriteLine("Missing config needed to send mail.");
                Environment.Exit(-1);
            }
            string user = Settings.sendFromAccount;
            string pw = Settings.sendFromAccountPw;

            string to = sendTo;
            string from = Settings.sendFrom == null ? "donotreply@unknown.net" : Settings.sendFrom;
            string subject = Settings.subject;

            // Prepare html document
            string body = @"<html><body>";
            // Style
            body += @"<style> pre { font-family: 'Consolas', 'Lucida Console', 'Courier New', monospace; } </style>";
            body += @"<style> p { font-family: 'Consolas', 'Lucida Console', 'Courier New', monospace; } </style>";

            // Header
            body += $"<p>{suite.name}<br />{DateTime.Now.ToString()}</p>";
            string outcomesString = string.Empty;
            foreach (string s in filters)
                outcomesString += s + " ";
            body += $"<p>Outcomes: {outcomesString}<br />ID pattern: {idPattern}</p>";
            body += "<hr />";

            // Main table
            //body += @"<p>" + Report.SuiteToTable(suite, true, Report.readFilters(filters), selection) + "</p>";
            var tests = Report.SelectTests(suite, filters, idPattern);
            string table = Report.TestsToTable(tests, true, Report.readCols(Settings.columns));
            body += table;

            body += @"</body></html>"; body += Environment.NewLine;
            // end: Prepare html document

            SendMail(to, from, subject, body, user, pw);
        
        }


        public static void SendMail(string to, string from, string subject, string body, string user, string pw)
        {

            SmtpClient client = new SmtpClient();
            client.Host = Settings.smtpHost;
            client.Port = Settings.smtpPort;
            client.Timeout = Settings.smtpTimeout;
            client.EnableSsl = Settings.enableSsl;
            client.UseDefaultCredentials = Settings.useDefaultCredentials;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential(user, pw);

            MailMessage mm = new MailMessage(from, to, subject, body);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.IsBodyHtml = true;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            try
            {
                client.Send(mm);
                Console.WriteLine("Email sent.");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Failed to send: {ex.Message}");
            }

            
        }

        
    }
}
