using System;
using System.Text.RegularExpressions;

namespace JenkinsLogParser.Tokens
{
  public class ProjectBuildEndline : IToken 
  {
    public Regex RegularExpression { get; set; }
    public Regex ReplaceRegularExpression { get; set; }
    public string Line { get; set; }
    public RegexOptions Options { get; set; }

    public ProjectBuildEndline()
    {
      Options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
      var halfSecondTimeout = new TimeSpan(0, 0, 0, 0, 500);
      RegularExpression = new Regex(@"(?<=Done Building Project.).*[\\](.*?)(?=\.csproj\"")", Options, halfSecondTimeout);
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
      var result = RegularExpression.Match(tempLogLine);
      if (result.Success)
      {
        Line = result.Groups[1].Value;// Value;
      }
      return result.Success;
    }

    public string GetLine()
    {
      return "Project:" + Line + " END ";
    }
  }
}
