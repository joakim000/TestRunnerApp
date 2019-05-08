﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRunnerLib;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace TestRunnerAppCon
{
    enum Col { id, name, kind, status, prio, runs, estimatedTime,
               previousDateTime, previousOutcome, previousRuntime, webDriverType, runNotes,
               message, failStep, eType, eMessage };

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
                    case Col.previousOutcome:
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

        public static string TestsToTable(List<TestModel> tests, bool html, Col[] cols)
        {
            string table = "No tests found.";

            if (tests.Count() < 1)
            {
                return table;
            }

            var headersList = new List<string>();
            var columnsList = new List<Func<TestModel, object>>();
            foreach (Col c in cols)
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
                    case Col.previousOutcome:
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
                if (s.Equals("Pass", StringComparison.OrdinalIgnoreCase))
                        filterList.Add(Outcome.Pass);
                else if (s.Equals("Fail", StringComparison.OrdinalIgnoreCase))
                    filterList.Add(Outcome.Fail);
                else if (s.Equals("Warning", StringComparison.OrdinalIgnoreCase))
                    filterList.Add(Outcome.Warning);
                else if (s.Equals("NotRun", StringComparison.OrdinalIgnoreCase))
                    filterList.Add(Outcome.NotRun);
            }
            return filterList.ToArray();
        }


        public static WebDriverType readWebDriver(string s)
        {

            if (s.Equals("Chrome", StringComparison.OrdinalIgnoreCase))
                return WebDriverType.Chrome;
            if (s.Equals("Firefox", StringComparison.OrdinalIgnoreCase))
                return WebDriverType.Firefox;

            return WebDriverType.None;
        }

        public static Col[] readCols(string[] cols)
        {
            var colList = new List<Col>();
            foreach (string s in cols)
            {
                if (s.Equals("id", StringComparison.OrdinalIgnoreCase))
                    colList.Add(Col.id);
                else if (s.Equals("name", StringComparison.OrdinalIgnoreCase))
                    colList.Add(Col.name);
                else if (s.Equals("totalRuns", StringComparison.OrdinalIgnoreCase))
                    colList.Add(Col.runs);
                else if (s.Equals("estimatedTime", StringComparison.OrdinalIgnoreCase))
                    colList.Add(Col.estimatedTime);
                else if (s.Equals("previousDateTime", StringComparison.OrdinalIgnoreCase))
                    colList.Add(Col.previousDateTime);
                else if (s.Equals("webDriverType", StringComparison.OrdinalIgnoreCase))
                    colList.Add(Col.webDriverType);
                else if (s.Equals("runNotes", StringComparison.OrdinalIgnoreCase))
                    colList.Add(Col.runNotes);
                else if (s.Equals("previousOutcome", StringComparison.OrdinalIgnoreCase))
                    colList.Add(Col.previousOutcome);
                else if (s.Equals("previousRuntime", StringComparison.OrdinalIgnoreCase))
                    colList.Add(Col.previousRuntime);
                else if (s.Equals("failStep", StringComparison.OrdinalIgnoreCase))
                    colList.Add(Col.failStep);
                else if (s.Equals("message", StringComparison.OrdinalIgnoreCase))
                    colList.Add(Col.message);
                else if (s.Equals("eType", StringComparison.OrdinalIgnoreCase))
                    colList.Add(Col.eType);
                else if (s.Equals("eMessage", StringComparison.OrdinalIgnoreCase))
                    colList.Add(Col.eMessage);

            }
            return colList.ToArray();
        }

        public static List<TestModel> SelectTests(SuiteModel suite, string[] filters, string idPattern)
        {
            var selected = new List<TestModel>();

            Outcome[] filter = readFilters(filters);
            if (filter.Length == 0)
                selected = suite.tests.ToList();
            else
                foreach (Outcome o in filter)
                    selected.AddRange(suite.tests.Where(x => x.previousOutcome == o));

            if (!string.IsNullOrEmpty(idPattern))
                selected.RemoveAll(test => !Regex.IsMatch(test.id, idPattern));

            return selected;
        }

        public static void ListTests(SuiteModel suite, string[] filters, string idPattern)
        {
            var tests = Report.SelectTests(suite, filters, idPattern);
            string s = Report.TestsToTable(tests, false, Report.readCols(Settings.columns));
            Console.WriteLine(s);

        }
        public static void ListTests(List<TestModel> tests)
        {
            string s = Report.TestsToTable(tests, false, Report.readCols(Settings.columns));
            Console.WriteLine(s);

        }

    }
}
