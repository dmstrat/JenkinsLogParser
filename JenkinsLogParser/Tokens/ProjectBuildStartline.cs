using System;
using System.Text.RegularExpressions;

namespace JenkinsLogParser.Tokens
{
  public class ProjectBuildStartLine : IToken 
  {
    public Regex RegularExpression { get; set; }
    public Regex ReplaceRegularExpression { get; set; }
    public string Line { get; set; }
    public RegexOptions Options { get; set; }
    public TimeSpan Timespan { get; set; }

    public ProjectBuildStartLine()
    {
      Options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
      var halfSecondTimeout = new TimeSpan(0, 0, 0, 0,500);
      RegularExpression = new Regex(@"(?<=[\.sln|\.csproj])(.*?is building.).*[\\](.*?)(?=\.csproj\"")", Options, halfSecondTimeout);
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

    public bool IsMatchForThisToken(string logLine)
    {
      var tempLogLine = logLine;
      tempLogLine= ReplaceRegularExpression.Replace(tempLogLine, String.Empty);
      try
      {
        var result = RegularExpression.Match(tempLogLine);
        if (result.Success)
        {
          Line = result.Groups[2].Value;
        }
        return result.Success;
      }
      catch (RegexMatchTimeoutException)
      {
        return false;
      }
    }

    public string GetLine()
    {
      return "Project:" + Line + " START ";
    }
  }
}
