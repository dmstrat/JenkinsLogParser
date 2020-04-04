using JenkinsLogParser.Events;
using JenkinsLogParser.Events.Projects;
using JenkinsLogParser.Handlers;
using JenkinsLogParser.Helpers;
using JenkinsLogParser.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using JenkinsLogParser.Reports;

namespace JenkinsLogParser
{
  public class Parser
  {
    private FileInfo _LogFileInfo;
    private FileInfo _OutputFileInfo;
    private IList<IToken> _TokenList = new List<IToken>();
    private IList<string> _Output = new List<string>();
    private IDictionary<Type, object> _Reports = new Dictionary<Type, object>();

    public void Parse(FileInfo logFileInfo, FileInfo outputFileInfo)
    {
      _LogFileInfo = logFileInfo;
      _OutputFileInfo = outputFileInfo;
      _TokenList = new List<IToken>();
      _Output = new List<string>();
      //_Reports = BuildReportList();

      //TODO: move the "wire up" portion of the code in Program.cs to here?

      using var streamReader = new StreamReader(_LogFileInfo.FullName);
      string line = null;
      long lineNumber = 1;
      while ((line = streamReader.ReadLine()) != null)
      {
        ProcessLogLine(lineNumber, line);
        lineNumber++;
      }
      //ProcessTokenList();
      //using var streamWriter = new StreamWriter(_OutputFileInfo.FullName, false);
      //ProcessReports(streamWriter);
      //WriteToStreamHelper.WriteOutputToStream(streamWriter, _Output);
      //WriteToStreamHelper.WriteWarningsToStream(streamWriter, WarningHandler.ProjectWarningCount);
      //TODO: add report handling here for output.
    }

    private void ProcessLogLine(long lineNumber, string logLine)
    {
      foreach (var token in TokenRegistry.Tokens)
      {
        var match = token.ProcessLine(lineNumber, logLine);
        if (match)
        {
          AddTokenToOutput(token);
          break;
        }
      }
    }

    private void AddTokenToOutput(IToken token)
    {
      var tokenClone = token.GetClone();
      _TokenList.Add(tokenClone);
    }

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
