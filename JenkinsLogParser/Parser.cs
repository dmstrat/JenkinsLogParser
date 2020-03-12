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

    public void Parse(FileInfo logFileInfo, FileInfo outputFileInfo)
    {
      _LogFileInfo = logFileInfo;
      _OutputFileInfo = outputFileInfo;
      _TokenList = new List<IToken>();
      _Output = new List<string>();

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
        }
      }
    }

    private void ProcessLogLine(string logLine)
    {
      foreach (var token in TokenRegistry.Tokens)
      {
        var match = token.IsMatchForThisToken(logLine);
        if (match)
        {
          AddTokenToOutput(token);
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
      var previousTimespanToken = -1;
      TimeSpan duration = TimeSpan.Zero;
      for (int i = 0; i < _TokenList.Count; i++)
      {
        var currentToken = _TokenList[i];
        _Output.Add(currentToken.GetLine());
        if (currentToken is IHasTimespan)
        {
          var hasPreviousTimestamp = previousTimespanToken > -1;
          if (hasPreviousTimestamp)
          {
            duration = ((IHasTimespan)currentToken).GetTimespan() -
                           ((IHasTimespan)_TokenList[previousTimespanToken]).GetTimespan();
            _Output[previousTimespanToken] += " => " + duration;
          }
          previousTimespanToken = i;
        }
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
