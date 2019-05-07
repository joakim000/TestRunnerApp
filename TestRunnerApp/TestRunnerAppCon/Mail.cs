using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRunnerLib;
using System.Net.Mail;

namespace TestRunnerAppCon
{
    enum Col { id, name, kind, status, prio, runs, estimatedTime,
               previousDateTime, previousOutCome, previousRuntime, webDriverType, runNotes,
               message, failStep, eType, eMessage };

    enum FilterOutcome { all, pass, fail, warning, error };

    class Mail
    {
        public static void SuiteStatus(SuiteModel suite)
        {
            Outcome[] filter = { Outcome.Fail, Outcome.Warning };
            
            Col[] selection = { Col.id, Col.name, Col.previousDateTime, Col.webDriverType, Col.previousOutCome,
                                Col.failStep, Col.message, Col.eType};

            string user = "TestApp.By.Unicus@gmail.com";
            string pw = "TrollBoll12!";

            string to = "joakim.odermalm@unicus.no";
            string from = "donotreply@gmail.com";
            string subject = "TestRunnerApp suite status " + DateTime.Now.ToString();

            string body = @"<html><body>";
            body += @"<style> pre { font-family: 'Consolas', 'Lucida Console', 'Courier New', monospace; } </style>";
            body += @"<style> p { font-family: 'Consolas', 'Lucida Console', 'Courier New', monospace; } </style>";
            body += Environment.NewLine;
            body += @"<p>" + SuiteToTable(suite, true, filter, selection) + "</p>";
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

        //public static string SuiteToTable_old(SuiteModel suite, bool html)
        //{
        //    string table = "No tests found.";

        //    if (suite.tests.Count() < 1)
        //    {
        //        return table;
        //    }

        //    string[] columns = { "ID", "Name", "Last run", "Last outcome", "Ex message" };

            
        //    if (html)
        //    {
        //        table = suite.tests.ToHtmlTable(columns,
        //                 t => t.id,
        //                 t => t.name,
        //                 t => t.previousDateTime == null ? string.Empty : t.previousDateTime.ToString(),
        //                 t => t.previousOutcome,
        //                  t => t.runs.Count() > 0 && t.runs.Last()?.resultObj?.eMessage != null ?
        //                         t.runs.Last()?.resultObj?.eMessage : string.Empty

        //             );
        //    }
        //    else
        //    {
        //        table = suite.tests.ToStringTable(columns,
        //                 t => t.id,
        //                 t => t.name,
        //                 t => t.previousDateTime == null ? string.Empty : t.previousDateTime.ToString(),
        //                 t => t.previousOutcome,
        //                 t => t.runs.Count() > 0 && t.runs.Last()?.resultObj?.eMessage != null ?
        //                         t.runs.Last()?.resultObj?.eMessage : string.Empty


        //             );
        //    }
        //    return table;

        //}

        public static string SuiteToTable(SuiteModel suite, bool html, Outcome[] filter, Col[] selection)
        {
            string table = "No tests found.";

            if (suite.tests.Count() < 1)
            {
                return table;
            }

            var tests = new List<TestModel>();
            foreach (Outcome o in filter)
            {
                tests.AddRange(suite.tests.Where(x => x.previousOutcome == o));
            }
            if (tests.Count() < 1)
            {
                return table;
            }

            var headersList = new List<string>();
            var columnsList = new List<Func<TestModel, object>>();

            foreach (Col c in selection)
            {
                switch (c)
                {
                    // From test
                    case Col.id:
                        headersList.Add("ID");
                        columnsList.Add(t => t.id);
                        break;
                    case Col.name:
                        headersList.Add("Name");
                        columnsList.Add(t => t.name);
                        break;
                    case Col.previousDateTime:
                        headersList.Add("Last run");
                        columnsList.Add(t => t.previousDateTime == null ? string.Empty : t.previousDateTime.ToString());
                        break;
                    case Col.previousOutCome:
                        headersList.Add("Last outcome");
                        columnsList.Add(t => t.previousOutcome);
                        break;
                    case Col.runs:
                        headersList.Add("Runs");
                        columnsList.Add(t => t.runs.Count());
                        break;
                    case Col.estimatedTime:
                        headersList.Add("Estimated time");
                        columnsList.Add(t => t.estimatedTime);
                        break;

                    // From run
                    case Col.previousRuntime:
                        headersList.Add("Run time");
                        columnsList.Add(t => t.runs.Count() > 0 && t.runs.Last()?.runTime != null ?
                                            t.runs.Last()?.runTime.ToString() : string.Empty);
                        break;

                    case Col.webDriverType:
                        headersList.Add("Web driver");
                        columnsList.Add(t => t.runs.Count() > 0 && t.runs.Last()?.webDriverType != null ?
                                            t.runs.Last()?.webDriverType.ToString() : string.Empty);
                        break;
                    case Col.runNotes:
                        headersList.Add("Last run note");
                        columnsList.Add(t => t.runs.Count() > 0 && t.runs.Last()?.note != null ?
                                            t.runs.Last()?.note : string.Empty);
                        break;

                    // From result
                    case Col.failStep:
                        headersList.Add("Step");
                        columnsList.Add(t => t.runs.Count() > 0 && t.runs.Last()?.resultObj?.failStep != null ?
                                            t.runs.Last()?.resultObj?.failStep.ToString() : string.Empty);
                        break;

                    case Col.message:
                        headersList.Add("Message");
                        columnsList.Add(t => t.runs.Count() > 0 && t.runs.Last()?.resultObj?.message != null ?
                                            t.runs.Last()?.resultObj?.message.ToString() : string.Empty);
                        break;
                    case Col.eType:
                        headersList.Add("Ex type");
                        columnsList.Add(t => t.runs.Count() > 0 && t.runs.Last()?.resultObj?.eType != null ?
                                            t.runs.Last()?.resultObj?.eType : string.Empty);
                        break;
                    case Col.eMessage:
                        headersList.Add("Ex message");
                        columnsList.Add(t => t.runs.Count() > 0 && t.runs.Last()?.resultObj?.eMessage != null ?
                                            t.runs.Last()?.resultObj?.eMessage : string.Empty);
                        break;

                    default:
                        break;
                }
            }

            var headers = headersList.ToArray();
            var columns = columnsList.ToArray();

            return html ? tests.ToHtmlTable(headers, columns) : tests.ToStringTable(headers, columns);
        }

    }
}
