using JenkinsLogParser.Events;
using JenkinsLogParser.Helpers;
using JenkinsLogParser.Tokens;
using System;
using System.Collections.Generic;
using System.IO;

namespace JenkinsLogParser
{
  public class Parser
  {
    private FileInfo _LogFileInfo;
    private FileInfo _OutputFileInfo;
    protected static IDictionary<Type, object> Reports = new Dictionary<Type, object>();

    public void Parse(FileInfo logFileInfo, FileInfo outputFileInfo)
    {
      _LogFileInfo = logFileInfo;
      _OutputFileInfo = outputFileInfo;
      Initialize();
      ProcessLog();
      ProcessReports();
    }

    private void Initialize()
    {
      Reports = ReportHelper.BuildReportDictionary();
      var delegates = DelegateHelper.BuildDelegateDictionary(Reports);
      TokenEvents.Actions = delegates;
    }

    private void ProcessLog()
    {
      using var streamReader = new StreamReader(_LogFileInfo.FullName);
      string line = null;
      long lineNumber = 1;
      while ((line = streamReader.ReadLine()) != null)
      {
        ProcessLogLine(lineNumber, line);
        lineNumber++;
      }
    }

    private void ProcessLogLine(long lineNumber, string logLine)
    {
      foreach (var token in TokenRegistry.Tokens)
      {
        var match = token.ProcessLine(lineNumber, logLine);
        if (match)
        {
          break;
        }
      }
    }

    private void ProcessReports()
    {
      ReportHelper.ProcessReports(Reports, _OutputFileInfo);
    }
  }
}
