using System;
using System.Text.RegularExpressions;
using JenkinsLogParser.Events;
using JenkinsLogParser.Events.Projects;

namespace JenkinsLogParser.Tokens
{
  public class ProjectBuildStartLine : IToken 
  {
    public Regex RegularExpression { get; set; }
    public Regex ReplaceRegularExpression { get; set; }
    public string Line { get; set; }
    public RegexOptions Options { get; set; }

    public ProjectBuildStartLine()
    {
      Options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
      var halfSecondTimeout = new TimeSpan(0, 0, 0, 0,500);
      RegularExpression = new Regex(@"(?<=[\.sln|\.csproj])(.*?is building.).*[\\](.*?)(?=\.(csproj|wixproj)\"")", Options, halfSecondTimeout);
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
          RaiseProjectStartedEvent(lineNumber, Line);
        }
        return result.Success;
      }
      catch (RegexMatchTimeoutException)
      {
        return false;
      }
    }

    private void RaiseProjectStartedEvent(long lineNumber, string line)
    {
      var args = BuildProjectStartedEventArgs(lineNumber, line);
      var projectStartedEvent = new ProjectStarted(args);
      TokenEvents.Raise(projectStartedEvent);
    }

    private ProjectStartedEventArgs BuildProjectStartedEventArgs(long lineNumber, string line)
    {
      var projectStartedEventArgs = new ProjectStartedEventArgs()
      {
        LineNumber = lineNumber,
        ProjectName = line
      };
      return projectStartedEventArgs;
    }

    public string GetLine()
    {
      return "Project:" + Line + " START ";
    }

    public string GetMatch()
    {
      return Line;
    }
  }
}
