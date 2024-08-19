using JenkinsLogParser.Events;
using JenkinsLogParser.Events.DotCover;
using JenkinsLogParser.Events.Projects;
using JenkinsLogParser.Handlers;
using JenkinsLogParser.Reports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JenkinsLogParser.Helpers
{
    internal static class DelegateHelper
  {
    public static IList<Delegate> BuildDelegateDictionary(IDictionary<Type, object> reports)
    {
      var projectBuildHierarchyReport = (ProjectBuildHierarchyReport)GetReport(reports, typeof(ProjectBuildHierarchyReport));
      var warningSummaryReport = (WarningSummaryReport)GetReport(reports, typeof(WarningSummaryReport));
      var warningByProjectSummaryReport = (WarningByProjectSummaryReport)GetReport(reports, typeof(WarningByProjectSummaryReport));
      var timespanItemsReport = (TimestampItemsReport) GetReport(reports, typeof(TimestampItemsReport));
      var dotCoverItemsReport = (DotCoverItemsReport)GetReport(reports, typeof(DotCoverItemsReport));

      var newDelegates = new List<Delegate>();

      var projectHandler = new ProjectHandler(ref projectBuildHierarchyReport);
      newDelegates.Add((Action<ProjectStarted>)projectHandler.Handle);
      newDelegates.Add((Action<ProjectEnded>)projectHandler.Handle);

      var warningHandler = new WarningHandler(ref warningSummaryReport, ref warningByProjectSummaryReport);
      newDelegates.Add((Action<ProjectStarted>)warningHandler.Handle);
      newDelegates.Add((Action<WarningAdded>)warningHandler.Handle);
      newDelegates.Add((Action<ProjectEnded>)warningHandler.Handle);

      var timespanHandler = new TimespanHandler(ref timespanItemsReport);
      newDelegates.Add((Action<TimestampAdded>) timespanHandler.Handle);

      var dotCoverHandler = new DotCoverHandler(ref dotCoverItemsReport);
      newDelegates.Add((Action<DotCoverStarted>) dotCoverHandler.Handle);
      newDelegates.Add((Action<DotCoverEnded>) dotCoverHandler.Handle);
      newDelegates.Add((Action<DotCoverCoverExecutionAdded>) dotCoverHandler.Handle);
      return newDelegates;
    }

    public static object GetReport(IDictionary<Type, object> reports, Type reportType)
    {
      var foundReport = reports.FirstOrDefault(r => r.Key == reportType).Value;
      var reportNotFound = foundReport == null;
      if (reportNotFound)
      {
        throw new Exception("Report Not registered!");
      }
      return Convert.ChangeType(foundReport, reportType);
    }

  }
}
