using System;
using System.Text.RegularExpressions;
using JenkinsLogParser.Events;
using JenkinsLogParser.Events.Projects;

namespace JenkinsLogParser.Tokens
{
  public class ProjectBuildEndLine : IToken 
  {
    public Regex RegularExpression { get; set; }
    public Regex ReplaceRegularExpression { get; set; }
    public string Line { get; set; }
    public RegexOptions Options { get; set; }

    public ProjectBuildEndLine()
    {
      Options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
      var halfSecondTimeout = new TimeSpan(0, 0, 0, 0, 500);
      RegularExpression = new Regex(@"(?<=Done Building Project.).*[\\](.*?)(?=\.(csproj|wixproj)\"")", Options, halfSecondTimeout);
      ReplaceRegularExpression = new Regex(@"^\s*|\s*$", Options);
    }

    public bool ProcessLine(long lineNumber, string logLine)
    {
      var tempLogLine = logLine;
      tempLogLine= ReplaceRegularExpression.Replace(tempLogLine, String.Empty);
      var result = RegularExpression.Match(tempLogLine);
      if (result.Success)
      {
        Line = result.Groups[1].Value;
        RaiseProjectEndedEvent(lineNumber, Line, logLine);
      }
      return result.Success;
    }

    private void RaiseProjectEndedEvent(long lineNumber, string line, string fullText)
    {
      var args = BuildProjectEndedEventArgs(lineNumber, line, fullText);
      var projectEndedEvent = new ProjectEnded(args);
      TokenEvents.Raise(projectEndedEvent);
    }

    private ProjectEndedEventArgs BuildProjectEndedEventArgs(long lineNumber, string line, string fullText)
    {
      var args = new ProjectEndedEventArgs()
      {
        LineNumber = lineNumber,
        FullText = fullText,
        RegExResult = line,
        ProjectName = line
      };
      return args;
    }
  }
}
