using System;
using System.Text.RegularExpressions;
using JenkinsLogParser.Events;
using JenkinsLogParser.Events.Projects;

namespace JenkinsLogParser.Tokens
{
  public class WarningLine : IToken 
  {
    public Regex RegularExpression { get; set; }
    public Regex ReplaceRegularExpression { get; set; }
    public string Line { get; set; }
    public RegexOptions Options { get; set; }

    public WarningLine()
    {
      Options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
      RegularExpression = new Regex(@"(?<=\:\s{1}warning\s)(.*?)(?=\:)", Options);
      ReplaceRegularExpression = new Regex(@"^\s*|\s*$", Options);
    }

    public bool ProcessLine(long lineNumber, string logLine)
    {
      var tempLogLine = logLine;
      tempLogLine= ReplaceRegularExpression.Replace(tempLogLine, String.Empty);
      var result = RegularExpression.Match(tempLogLine);
      if (result.Success)
      {
        Line = result.Value;
        RaiseWarningAddedEvent(lineNumber, Line, logLine);
      }
      return result.Success;
    }

    private void RaiseWarningAddedEvent(long lineNumber, string line, string fullText)
    {
      var args = BuildWarningAddedEventArgs(lineNumber, line, fullText);
      var timestampAddedEvent = new WarningAdded(args);
      TokenEvents.Raise(timestampAddedEvent);
    }

    private WarningAddedEventArgs BuildWarningAddedEventArgs(long lineNumber, string line, string fullText)
    {
      var args = new WarningAddedEventArgs()
      {
        LineNumber = lineNumber,
        FullText = fullText,
        RegExResult = line,
        WarningName = line
      };
      return args;
    }
  }
}
