using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace ParserConsole
{
  internal class Options
  {
    [Option('i', "inputFile", Required = true, HelpText = ".txt Input file to be processed.")]
    public string InputFile { get; set; }

    [Option('o', "outputFile", Required = true, HelpText = ".txt Output file for resulting report.")]
    public string OutputFile { get; set; }
  }
}
