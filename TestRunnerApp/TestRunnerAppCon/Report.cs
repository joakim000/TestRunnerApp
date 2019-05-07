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

    class Report
    {
        public static string SuiteToTable(SuiteModel suite, bool html, Outcome[] filter, Col[] selection)
        {
            string table = "No tests found.";

            if (suite.tests.Count() < 1)
            {
                return table;
            }

            var tests = new List<TestModel>();
            if (filter.Length == 0)
                tests = suite.tests.ToList();
            else
            {
                foreach (Outcome o in filter)
                {
                    tests.AddRange(suite.tests.Where(x => x.previousOutcome == o));
                }
                if (tests.Count() < 1)
                {
                    return table;
                }
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

        public static Outcome[] readFilters(string[] filters)
        {
            var filterList = new List<Outcome>();
            foreach (string s in filters)
            {
                switch (s)
                {
                    case "Pass":
                        filterList.Add(Outcome.Pass);
                        break;
                    case "Fail":
                        filterList.Add(Outcome.Fail);
                        break;
                    case "Warning":
                        filterList.Add(Outcome.Warning);
                        break;
                    case "NotRun":
                        filterList.Add(Outcome.NotRun);
                        break;
                    default:
                        break;
                }
            }
            return filterList.ToArray();
        }

        public static Col[] readCols(string[] cols)
        {
            var colList = new List<Col>();
            foreach (string s in cols)
            {
                switch (s)
                {
                    case "id":
                        colList.Add(Col.id);
                        break;
                   
                    default:
                        break;
                }
            }
            return colList.ToArray();
        }


    }
}
