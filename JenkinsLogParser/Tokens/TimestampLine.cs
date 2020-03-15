using System;
using System.Text.RegularExpressions;
using JenkinsLogParser.Helpers;

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
        Line = result.Value;
        Timespan = TimeHelper.GenerateTimestampFromLine(Line);
      }
      return result.Success;
    }

    public TimeSpan GetTimespan()
    {
      return Timespan;
    }

    public string GetLine()
    {
      return Line;
    }

    public string GetMatch()
    {
      return Line;
    }
  }
}
