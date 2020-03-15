using JenkinsLogParser.Events;
using JenkinsLogParser.Events.Projects;
using JenkinsLogParser.Handlers;
using JenkinsLogParser.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JenkinsLogParser
{
  public class Parser
  {
    private FileInfo _LogFileInfo;
    private FileInfo _OutputFileInfo;
    private IList<IToken> _TokenList = new List<IToken>();
    private IList<string> _Output = new List<string>();
    private IDictionary<string, int> _WarningCount = new Dictionary<string, int>();

    public void Parse(FileInfo logFileInfo, FileInfo outputFileInfo)
    {
      _LogFileInfo = logFileInfo;
      _OutputFileInfo = outputFileInfo;
      _TokenList = new List<IToken>();
      _Output = new List<string>();
      _WarningCount = new Dictionary<string, int>();

      using var streamReader = new StreamReader(_LogFileInfo.FullName);
      string line = null;
      while ((line = streamReader.ReadLine()) != null)
      {
        ProcessLogLine(line);
      }
      ProcessTokenList();
      using var streamWriter = new StreamWriter(_OutputFileInfo.FullName, false);
      WriteOutputToStream(streamWriter);
      WriteWarningsToStream(streamWriter);
    }

    private void ProcessLogLine(string logLine)
    {
      foreach (var token in TokenRegistry.Tokens)
      {
        var match = token.IsMatchForThisToken(logLine);
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

    private void ProcessTokenList()
    {
      var indent = 0;
      var previousTimespanToken = -1;
      TimeSpan duration = TimeSpan.Zero;
      for (int i = 0; i < _TokenList.Count; i++)
      {
        var currentToken = _TokenList[i];
        var currentLineOutput = currentToken.GetLine();
        if (currentToken is WarningLine)
        {
          var newWarning = new WarningAdded()
          { 
            WarningName = currentToken.GetMatch()
          };
          TokenEvents.Raise(newWarning);
        }

        if (currentToken is IHasTimespan)
        {
          var hasPreviousTimestamp = previousTimespanToken > -1;
          if (hasPreviousTimestamp)
          {
            var currentTimespan = ((IHasTimespan) currentToken).GetTimespan();
            var previousTimespan = ((IHasTimespan) _TokenList[previousTimespanToken]).GetTimespan();
            duration = currentTimespan - previousTimespan;
            currentLineOutput += " => " + duration;
          }
          previousTimespanToken = i;
        }

        if (_TokenList[i] is ProjectBuildEndLine)
        {
          var newEvent = new ProjectEnded()
          {
            ProjectName = _TokenList[i].GetMatch()
          };
          TokenEvents.Raise(newEvent);
          indent--;
        }

        if (_TokenList[i].PrintIndividualLine())
        {
          var spaces = new string(' ',2*indent);
          _Output.Add(spaces + currentLineOutput);
        }

        if (_TokenList[i] is ProjectBuildStartLine)
        {
          var newEvent = new ProjectStarted()
          {
            ProjectName = _TokenList[i].GetMatch()
          };
          TokenEvents.Raise(newEvent);
          indent++;
        }
      }
    }

    private void WriteWarningsToStream(StreamWriter streamWriter)
    {
      WriteLineToOutput(streamWriter, "Warnings for this BUILD (via Events)");
      var externalList = WarningHandler.ProjectWarningCount["EXTERNAL"];
      var projectList = WarningHandler.ProjectWarningCount.Where(x => x.Key != "EXTERNAL");
      var sortedProjectList = projectList.OrderBy(x => x.Key);
      foreach (var projectWarningList in sortedProjectList)
      {
        var worthPrinting = projectWarningList.Value.Count > 0;
        if (worthPrinting)
        {
          var currentProject = projectWarningList.Key;
          WriteLineToOutput(streamWriter, currentProject);
          var warningList = projectWarningList.Value;
          var sortedWarningList = from pair in warningList orderby pair.Key select pair;
          foreach (var warningCount in sortedWarningList)
          {
            var outputLine = "  " + warningCount.Key + ":" + warningCount.Value;
            WriteLineToOutput(streamWriter, outputLine);
          }
        }
      }

      WriteLineToOutput(streamWriter, "EXTERNAL TO PROJECTS");
      foreach (var warningItem in externalList)
      {
        var outputLine = "  " + warningItem.Key + ":" + warningItem.Value;
        WriteLineToOutput(streamWriter, outputLine);
      }
    }

    private void WriteOutputToStream(StreamWriter streamWriter)
    {
      foreach (string line in _Output)
      {
        WriteLineToOutput(streamWriter, line);
      }
    }

    private static void WriteLineToOutput(StreamWriter streamWriter, string lineToRecord)
    {
      streamWriter.WriteLine(lineToRecord);
    }
  }
}
