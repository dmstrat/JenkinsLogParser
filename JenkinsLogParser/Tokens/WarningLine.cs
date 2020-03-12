using System;
using System.Text.RegularExpressions;

namespace JenkinsLogParser.Tokens
{
  public class WarningLine : IToken //, IHasTimespan
  {
    public Regex RegularExpression { get; set; }
    public Regex ReplaceRegularExpression { get; set; }
    public string Line { get; set; }
    public RegexOptions Options { get; set; }
    public TimeSpan Timespan { get; set; }

    public WarningLine()
    {
      Options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
      RegularExpression = new Regex(@"(?<=\:\s{1}warning\s)(.*?)(?=\:)", Options);
      ReplaceRegularExpression = new Regex(@"^\s*|\s*$", Options);
    }

    public IToken GetClone()
    {
      return (IToken)MemberwiseClone();
    }

    public bool PrintIndividualLine()
    {
      return false;
    }

    public bool IsMatchForThisToken(string logLine)
    {
      var tempLogLine = logLine;
      tempLogLine= ReplaceRegularExpression.Replace(tempLogLine, String.Empty);
      var result = RegularExpression.Match(tempLogLine);
      if (result.Success)
      {
        Line = result.Value;
        //GenerateTimestampFromLine();
      }
      return result.Success;
    }

    public string GetLine()
    {
      return Line;
    }
  }
}
