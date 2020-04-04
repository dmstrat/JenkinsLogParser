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
      return newDictionary;
    }

    public static void ProcessReports(IDictionary<Type, object> reports, FileInfo outputFileInfo)
    {
      using (var streamWriter = new StreamWriter(outputFileInfo.FullName, false))
      {
        var projectBuildReport = (ProjectBuildHierarchyReport)DelegateHelper.GetReport(reports, typeof(ProjectBuildHierarchyReport));
        streamWriter.WriteLine("-------------Report[START]:{0}-------------------", "ProjectBuildHierarchy");
        foreach (var row in projectBuildReport)
        {
          streamWriter.WriteLine(row);
        }
        streamWriter.WriteLine("-------------Report[END]:{0}-------------------", "ProjectBuildHierarchy");
      }
    }
  }
}
