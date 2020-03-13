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

      using (var streamReader = new StreamReader(_LogFileInfo.FullName))
      {
        string line = null;
        while ((line = streamReader.ReadLine()) != null)
        {
          ProcessLogLine(line);
        }
        ProcessTokenList();
        using (var streamWriter = new StreamWriter(_OutputFileInfo.FullName, false))
        {
          WriteOutputToStream(streamWriter);
          WriteWarningsToStream(streamWriter);
        }
      }
    }

    private void ProcessLogLine(string logLine)
    {
      var matchCount = 0;
      foreach (var token in TokenRegistry.Tokens)
      {
        var match = token.IsMatchForThisToken(logLine);
        if (match)
        {
          matchCount++;
          AddTokenToOutput(token);
        }
      }

      if (matchCount > 1)
      {
        System.Diagnostics.Debugger.Break();
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
          var tempToken = currentToken.GetClone();
          AddToWarningCount(tempToken);
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


        if (_TokenList[i] is ProjectBuildEndline)
        {
          indent--;
        }

        if (_TokenList[i].PrintIndividualLine())
        {
          var spaces = new string(' ',2*indent);
          _Output.Add(spaces + currentLineOutput);
        }

        if (_TokenList[i] is ProjectBuildStartLine)
        {
          indent++;
        }

      }
    }

    private void AddToWarningCount(IToken token)
    {
      var warningAsString = token.GetLine().Trim();
      var warningHasValue = warningAsString.Length > 0;
      if (warningHasValue)
      {
        var isInDictionaryAlready = _WarningCount.Keys.Contains(warningAsString);
        if (isInDictionaryAlready)
        {
          _WarningCount[warningAsString]++;
        }
        else
        {
          _WarningCount.Add(warningAsString, 1);
        }
      }
    }

    private void WriteWarningsToStream(StreamWriter streamWriter)
    {
      WriteLineToOutput(streamWriter, "Warnings for this BUILD");
      foreach (KeyValuePair<string, int> kvp in _WarningCount)
      {
        var lineToWrite = kvp.Key + ":=>" + (kvp.Value/2);
        WriteLineToOutput(streamWriter, lineToWrite);
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
