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

      var warningByProjectSummaryReport = new WarningByProjectSummaryReport();
      newDictionary.Add(typeof(WarningByProjectSummaryReport), warningByProjectSummaryReport);

      var timespanItemsReport = new TimestampItemsReport();
      newDictionary.Add(typeof(TimestampItemsReport), timespanItemsReport);

      var dotCoverItemsReport = new DotCoverItemsReport();
      newDictionary.Add(typeof(DotCoverItemsReport), dotCoverItemsReport);

      return newDictionary;
    }

    public static void ProcessReports(IDictionary<Type, object> reports, FileInfo outputFileInfo)
    {
      using var streamWriter = new StreamWriter(outputFileInfo.FullName, false);
      ProcessProjectBuildHierarchyReport(streamWriter, reports);
      ProcessWarningSummaryReport(streamWriter, reports);
      ProcessWarningsByProjectReport(streamWriter, reports);
      ProcessTimestampItemsReport(streamWriter, reports);
      ProcessDotCoverItemsReport(streamWriter, reports);
    }

    private static void ProcessProjectBuildHierarchyReport(StreamWriter streamWriter, IDictionary<Type, object> reports)
    {
      var projectBuildReport = (ProjectBuildHierarchyReport)DelegateHelper.GetReport(reports, typeof(ProjectBuildHierarchyReport));
      var reportName = projectBuildReport.GetReportName();
      var rowHeaders = projectBuildReport.GetReportRowHeaders();
      var projectBuildReportRows = projectBuildReport.GetReportRows();
      ProcessGenericReport(streamWriter, reportName, rowHeaders, projectBuildReportRows);
    }

    private static void ProcessWarningSummaryReport(StreamWriter streamWriter, IDictionary<Type, object> reports)
    {
      var warningSummaryReport =
        (WarningSummaryReport)DelegateHelper.GetReport(reports, typeof(WarningSummaryReport));
      var reportName = warningSummaryReport.GetReportName();
      var rowHeaders = warningSummaryReport.GetReportRowHeaders();
      var warningSummaryReportRows = warningSummaryReport.GetReportRows();
      ProcessGenericReport(streamWriter, reportName, rowHeaders, warningSummaryReportRows);
    }

    private static void ProcessWarningsByProjectReport(StreamWriter streamWriter, IDictionary<Type, object> reports)
    {
      var warningByProjectSummaryReport =
        (WarningByProjectSummaryReport)DelegateHelper.GetReport(reports, typeof(WarningByProjectSummaryReport));
      var reportName = warningByProjectSummaryReport.GetReportName();
      var rowHeaders = warningByProjectSummaryReport.GetReportRowHeaders();
      var warningByProjectSummaryReportRows = warningByProjectSummaryReport.GetReportRows();
      ProcessGenericReport(streamWriter, reportName, rowHeaders, warningByProjectSummaryReportRows);
    }

    private static void ProcessTimestampItemsReport(StreamWriter streamWriter, IDictionary<Type, object> reports)
    {
      var timestampItemsReport = (TimestampItemsReport)DelegateHelper.GetReport(reports, typeof(TimestampItemsReport));
      var reportName = timestampItemsReport.GetReportName();
      var rowHeaders = timestampItemsReport.GetReportRowHeaders();
      var timestampItemsReportRows = timestampItemsReport.GetReportRows();
      ProcessGenericReport(streamWriter, reportName, rowHeaders, timestampItemsReportRows);
    }

    private static void ProcessDotCoverItemsReport(StreamWriter streamWriter, IDictionary<Type, object> reports)
    {
      var dotCoverItemsReport = (DotCoverItemsReport)DelegateHelper.GetReport(reports, typeof(DotCoverItemsReport));
      var reportName = dotCoverItemsReport.GetReportName();
      var rowHeaders = dotCoverItemsReport.GetReportRowHeaders();
      var dotCoverItemsReportRows = dotCoverItemsReport.GetReportRows();
      ProcessGenericReport(streamWriter, reportName, rowHeaders, dotCoverItemsReportRows);
    }

    private static void ProcessGenericReport(StreamWriter streamWriter, string reportName,string rowHeaders, IList<string> rows)
    {
      streamWriter.WriteLine("-------------Report[START]:{0}-------------------", reportName);
      streamWriter.WriteLine(rowHeaders);
      foreach (var row in rows)
      {
        streamWriter.WriteLine(row);
      }
      streamWriter.WriteLine("-------------Report[END]:{0}-------------------", reportName);
    }

  }
}
