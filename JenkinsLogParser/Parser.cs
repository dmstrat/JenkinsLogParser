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
    protected static IList<Delegate> Delegates = new List<Delegate>();

    public void Parse(FileInfo logFileInfo, FileInfo outputFileInfo)
    {
      Initialize();
      _LogFileInfo = logFileInfo;
      _OutputFileInfo = outputFileInfo;

      using var streamReader = new StreamReader(_LogFileInfo.FullName);
      string line = null;
      long lineNumber = 1;
      while ((line = streamReader.ReadLine()) != null)
      {
        ProcessLogLine(lineNumber, line);
        lineNumber++;
      }
      ReportHelper.ProcessReports(Reports, _OutputFileInfo);
    }

    private void Initialize()
    {
      Reports = ReportHelper.BuildReportDictionary();
      Delegates = DelegateHelper.BuildDelegateDictionary(Reports);
      TokenEvents.Actions = Delegates;
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


    // KEEPING UNTIL CONVERTED IN WHOLE
    //private void ProcessTokenList()
    //{
    //  var indent = 0;
    //  var previousTimespanToken = -1;
    //  TimeSpan duration = TimeSpan.Zero;
    //  for (int i = 0; i < _TokenList.Count; i++)
    //  {
    //    var currentToken = _TokenList[i];
    //    var currentLineOutput = currentToken.GetLine();
    //    if (currentToken is WarningLine)
    //    {
    //      var warningAddedEvent = new WarningAdded()
    //      { 
    //        WarningName = currentToken.GetMatch()
    //      };
    //      TokenEvents.Raise(warningAddedEvent);
    //    }

    //    if (currentToken is IHasTimespan)
    //    {
    //      var hasPreviousTimestamp = previousTimespanToken > -1;
    //      if (hasPreviousTimestamp)
    //      {
    //        var currentTimespan = ((IHasTimespan) currentToken).GetTimespan();
    //        var previousTimespan = ((IHasTimespan) _TokenList[previousTimespanToken]).GetTimespan();
    //        duration = currentTimespan - previousTimespan;
    //        currentLineOutput += " => " + duration;
    //      }
    //      previousTimespanToken = i;
    //    }

    //    if (_TokenList[i] is ProjectBuildEndLine)
    //    {
    //      var projectEndedEvent = new ProjectEnded()
    //      {
    //        ProjectName = _TokenList[i].GetMatch()
    //      };
    //      TokenEvents.Raise(projectEndedEvent);
    //      indent--;
    //    }

    //    if (_TokenList[i].PrintIndividualLine())
    //    {
    //      var spaces = new string(' ',2*indent);
    //      _Output.Add(spaces + currentLineOutput);
    //    }

    //    if (_TokenList[i] is ProjectBuildStartLine)
    //    {
    //      var projectStartedEvent = new ProjectStarted()
    //      {
    //        ProjectName = _TokenList[i].GetMatch()
    //      };
    //      TokenEvents.Raise(projectStartedEvent);
    //      indent++;
    //    }
    //  }
    //}



  }
}
