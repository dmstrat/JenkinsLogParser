using System;
using System.Text.RegularExpressions;
using JenkinsLogParser.Events;
using JenkinsLogParser.Events.Projects;
using JenkinsLogParser.Reports;

namespace JenkinsLogParser.Tokens
{
  public class JetBrainsDotCoverStartLine : IToken 
  {
    public Regex RegularExpression { get; set; }
    public Regex ReplaceRegularExpression { get; set; }
    public string Line { get; set; }
    public RegexOptions Options { get; set; }

    public JetBrainsDotCoverStartLine()
    {
      Options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
      var halfSecondTimeout = new TimeSpan(0, 0, 0, 0,500);
      RegularExpression = new Regex(@"^\s*(\[JetBrains dotCover\] Coverage session started*)(\s\[\d+\/\d+\/\d+\s\d+:\d+:\d+\s[AP]M])", Options, halfSecondTimeout);
      ReplaceRegularExpression = new Regex(@"^\s*|\s*$", Options);
    }

    public IToken GetClone()
    {
      return (IToken)MemberwiseClone();
    }

    public bool PrintIndividualLine()
    {
      return true;
    }

    public bool ProcessLine(long lineNumber, string logLine)
    {
      var tempLogLine = logLine;
      tempLogLine= ReplaceRegularExpression.Replace(tempLogLine, String.Empty);
      try
      {
        var result = RegularExpression.Match(tempLogLine);
        if (result.Success)
        {
          Line = result.Groups[2].Value;
          RaiseDotCoverStartedEvent(lineNumber, Line, logLine);
        }
        return result.Success;
      }
      catch (RegexMatchTimeoutException)
      {
        return false;
      }
    }

    private void RaiseDotCoverStartedEvent(long lineNumber, string line, string fullText)
    {
      var args = BuildDotCoverStartedEventArgs(lineNumber, line, fullText);
      var dotCoverStartedEvent = new DotCoverStarted(args);
      TokenEvents.Raise(dotCoverStartedEvent);
    }

    private DotCoverStartedEventArgs BuildDotCoverStartedEventArgs(long lineNumber, string line, string fullText)
    {
      var dotCoverStartedEventArgs = new DotCoverStartedEventArgs()
      {
        LineNumber = lineNumber,
        RegExResult = line, 
        FullText = fullText,
        ProjectName = line,
        Timestamp = GenerateTimestampFromLine(line),
        Action = DotCoverAction.Start
      };
      return dotCoverStartedEventArgs;
    }

    private DateTime GenerateTimestampFromLine(string input)
    {
      var regex = new Regex("\\d+\\/\\d+\\/\\d+\\s\\d+:\\d+:\\d+\\s[AP]M");
      var result = regex.Match(input);
      var line = string.Empty;
      if (result.Success)
      {
        line = result.Groups[0].Value;
      }

      var dateTime = Convert.ToDateTime(line);

      return dateTime;
    }
  }
}
