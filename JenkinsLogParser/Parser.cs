using System;
using System.IO;
using JenkinsLogParser.Tokens;

namespace JenkinsLogParser
{
  public class Parser
  {
    private FileInfo _LogFileInfo;
    private FileInfo _OutputFileInfo;

    public void Parse(FileInfo logFileInfo, FileInfo outputFileInfo)
    {
      _LogFileInfo = logFileInfo;
      _OutputFileInfo = outputFileInfo;

      using (var streamReader = new StreamReader(_LogFileInfo.FullName))
      {
        using (var streamWriter = new StreamWriter(_OutputFileInfo.FullName,false))
        {
          string line, prevLine = null;
          while ((line = streamReader.ReadLine()) != null)
          {
            if (prevLine == null)
            {
              prevLine = line;
              continue;
            }

            string lineToWrite;
            foreach (var token in TokenRegistry.Tokens)
            {
              var match = token.ProcessLineImpl(prevLine);// ProcessLine(prevLine);

              if (match)
              {
                lineToWrite = token.GetLine();
                WriteLineToOutput(streamWriter, lineToWrite);
              }
            }

            prevLine = line;
            //prevLineTimestamp = lineTimestamp;
          }
          WriteLineToOutput(streamWriter, line);
        }
      }
    }

    private static void WriteLineToOutput(StreamWriter streamWriter, string lineToRecord)
    {
      streamWriter.WriteLine(lineToRecord);
    }
  }
}
