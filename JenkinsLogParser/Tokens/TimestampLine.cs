using JenkinsLogParser.Events;
using JenkinsLogParser.Helpers;
using System;
using System.Text.RegularExpressions;

namespace JenkinsLogParser.Tokens
{
  public class TimestampLine : IToken, IHasTimespan
  {
    public Regex RegularExpression { get; set; }
    public Regex ReplaceRegularExpression { get; set; }
    public string Line { get; set; }
    public RegexOptions Options { get; set; }
    public TimeSpan Timespan { get; set; }

    public TimestampLine()
    {
      Options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
      RegularExpression = new Regex(@"^\s*(\${3}.*)", Options);
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
        Timespan = TimeHelper.GenerateTimespanFromLine(Line);
        RaiseTimestampAddedEvent(lineNumber, Line, logLine);
      }
      return result.Success;
    }

    private void RaiseTimestampAddedEvent(long lineNumber, string line, string fullText)
    {
      var args = BuildTimestampAddedEventArgs(lineNumber, line, fullText);
      var timestampAddedEvent = new TimestampAdded(args);
      TokenEvents.Raise(timestampAddedEvent);
    }

    private TimestampAddedEventArgs BuildTimestampAddedEventArgs(long lineNumber, string line, string fullText)
    {
      var args = new TimestampAddedEventArgs()
      {
        LineNumber = lineNumber,
        FullText = fullText,
        RegExResult = line,
        TimeSpan = TimeHelper.GenerateTimespanFromLine(line)
      };
      return args;
    }

    public TimeSpan GetTimespan()
    {
      return Timespan;
    }
  }
}
