using System;
using System.IO;

namespace ParserConsole
{
  class Program
  {
    private static FileInfo _LogFileInfo;
    private static FileInfo _OutputFileInfo;

    static void Main(string[] args)
    {
      VerifyArguments(args);
      var parser = new JenkinsLogParser.Parser(); 
      parser.Parse(_LogFileInfo, _OutputFileInfo);
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
