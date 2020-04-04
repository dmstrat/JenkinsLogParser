using JenkinsLogParser.Reports;
using System;
using System.Collections.Generic;
using System.IO;

namespace JenkinsLogParser.Helpers
{
  internal static class ReportHelper
  {
    public static IDictionary<Type, object> BuildReportDictionary()
    {
      var newDictionary = new Dictionary<Type, object>();

      var projectBuildHierarchyReport = new ProjectBuildHierarchyReport();
      newDictionary.Add(typeof(ProjectBuildHierarchyReport), projectBuildHierarchyReport);

      var warningSummaryReport = new WarningSummaryReport();
      newDictionary.Add(typeof(WarningSummaryReport), warningSummaryReport);

      return newDictionary;
    }

    public static void ProcessReports(IDictionary<Type, object> reports, FileInfo outputFileInfo)
    {
      using (var streamWriter = new StreamWriter(outputFileInfo.FullName, false))
      {
        ProcessProjectBuildHierarchyReport(streamWriter, reports);
        ProcessWarningSummaryReport(streamWriter, reports);
      }
    }
    
    private static void ProcessProjectBuildHierarchyReport(StreamWriter streamWriter, IDictionary<Type, object> reports)
    {
      var projectBuildReport = (ProjectBuildHierarchyReport)DelegateHelper.GetReport(reports, typeof(ProjectBuildHierarchyReport));
      var projectBuildReportRows = projectBuildReport.GetReportRows();
      ProcessGenericReport(streamWriter, "Warning Summary", projectBuildReportRows);
    }

    private static void ProcessWarningSummaryReport(StreamWriter streamWriter, IDictionary<Type, object> reports)
    {
      var warningSummaryReport =
        (WarningSummaryReport)DelegateHelper.GetReport(reports, typeof(WarningSummaryReport));
      var warningSummaryReportRows = warningSummaryReport.GetReportRows();
      ProcessGenericReport(streamWriter, "Warning Summary", warningSummaryReportRows);
    }

    private static void ProcessGenericReport(StreamWriter streamWriter, string reportName, IList<string> rows)
    {
      streamWriter.WriteLine("-------------Report[START]:{0}-------------------", reportName);
      foreach (var row in rows)
      {
        streamWriter.WriteLine(row);
      }
      streamWriter.WriteLine("-------------Report[END]:{0}-------------------", reportName);
    }

  }
}
