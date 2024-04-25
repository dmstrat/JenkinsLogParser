using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace ParserConsole
{
  class Program
  {
    private static FileInfo _LogFileInfo;
    private static FileInfo _OutputFileInfo;

    static void Main(string[] args)
    {
      Parser.Default.ParseArguments<Options>(args)
        .WithParsed(VerifyArguments)
        .WithNotParsed(HandleParseError);

      var parser = new JenkinsLogParser.Parser(); 
      parser.Parse(_LogFileInfo, _OutputFileInfo);
    }

    private static void HandleParseError(IEnumerable<Error> obj)
    {
      Console.WriteLine("Exception parsing the options provided.");
    }

    private static void VerifyArguments(Options options)
    {
      VerifyFileDetails(options.InputFile, out _LogFileInfo);
      _OutputFileInfo = new FileInfo(options.OutputFile);
    }

    private static void VerifyFileDetails(string fileName, out FileInfo outputFileInfo)
    {
      try
      {
        var testFile = new FileInfo(fileName);
        if (!testFile.Exists)
        {
          throw new Exception("First Argument (Source File) NOT an existing file [" + fileName + "]");
        }

        outputFileInfo = testFile;

      }
      catch (Exception e)
      {
        Console.WriteLine("Exception attempting to load filename to FileInfo!");
        Console.WriteLine(e);
        throw;
      }
    }
  }
}
