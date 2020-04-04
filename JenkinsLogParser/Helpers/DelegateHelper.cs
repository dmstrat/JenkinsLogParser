using JenkinsLogParser.Events;
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
      var projectBuildHierarchyReport = (ProjectBuildHierarchyReport)GetReport(reports, typeof(ProjectBuildHierarchyReport));// new ProjectBuildHierarchyReport();
      var newDelegates = new List<Delegate>();

      var lineHandler = new LineHandler();
      newDelegates.Add((Action<LineAdded>)lineHandler.Handle);

      var projectHandler = new ProjectHandler(ref projectBuildHierarchyReport);
      newDelegates.Add((Action<ProjectStarted>)projectHandler.Handle);
      newDelegates.Add((Action<WarningAdded>)projectHandler.Handle);
      newDelegates.Add((Action<ProjectEnded>)projectHandler.Handle);

      var warningHandler = new WarningHandler();
      newDelegates.Add((Action<ProjectStarted>)warningHandler.Handle);
      newDelegates.Add((Action<WarningAdded>)warningHandler.Handle);
      newDelegates.Add((Action<ProjectEnded>)warningHandler.Handle);

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
