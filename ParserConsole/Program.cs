using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JenkinsLogParser.Events;
using JenkinsLogParser.Events.Projects;
using JenkinsLogParser.Handlers;
using JenkinsLogParser.Reports;
using JenkinsLogParser.Tokens;

namespace ParserConsole
{
  class Program
  {
    private static FileInfo _LogFileInfo;
    private static FileInfo _OutputFileInfo;
    protected static IDictionary<Type, object> _Reports = new Dictionary<Type, object>();
    protected static IList<Delegate> _Delegates = new List<Delegate>();

    static void Main(string[] args)
    {
      VerifyArguments(args);
      _Reports = BuildReportDictionary();
      _Delegates = BuildDelegates();
      TokenEvents.Actions = _Delegates;
      var parser = new JenkinsLogParser.Parser(); 
      parser.Parse(_LogFileInfo, _OutputFileInfo);
      ProcessReports();
    }

    private static void ProcessReports()
    {
      using (var streamWriter = new StreamWriter(_OutputFileInfo.FullName, false))
      {
        var projectBuildReport = (ProjectBuildHierarchyReport) GetReport(typeof(ProjectBuildHierarchyReport));
        streamWriter.WriteLine("-------------Report[START]:{0}-------------------", "ProjectBuildHierarchy");
        foreach (var row in projectBuildReport)
        {
          streamWriter.WriteLine(row);
        }
        streamWriter.WriteLine("-------------Report[END]:{0}-------------------", "ProjectBuildHierarchy");
      }
    }

    private static IDictionary<Type, object> BuildReportDictionary()
    {
      var newDictionary = new Dictionary<Type, object>();
      var projectBuildHierarchyReport = new ProjectBuildHierarchyReport();
      newDictionary.Add(typeof(ProjectBuildHierarchyReport), projectBuildHierarchyReport);
      return newDictionary;
    }

    private static IList<Delegate> BuildDelegates()
    {
      var projectBuildHierarchyReport = (ProjectBuildHierarchyReport)GetReport(typeof(ProjectBuildHierarchyReport));// new ProjectBuildHierarchyReport();
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

    private static object GetReport(Type reportType)
    {
      var foundReport = _Reports.FirstOrDefault(r => r.Key == reportType).Value;
      var reportNotFound = foundReport == null;
      if (reportNotFound)
      {
        throw new Exception("Report Not registered!");
      }
      return Convert.ChangeType(foundReport, reportType);
    }

    static void VerifyArguments(string[] args)
    {
      if (args.Length > 1 && args.Length < 3)
      {
        _LogFileInfo = new FileInfo(args[0]);
        if (!_LogFileInfo.Exists)
        {
          throw new Exception("First Argument (Source File) NOT an existing file [" + args[0] + "]");
        }
        _OutputFileInfo = new FileInfo(args[1]);
      }
      else
      {
        throw new Exception("Incorrect number of arguments");
      }
    }
  }
}
