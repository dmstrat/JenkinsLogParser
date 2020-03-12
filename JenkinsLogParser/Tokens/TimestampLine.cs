using System;
using System.Text.RegularExpressions;
using JenkinsLogParser.Helpers;

namespace JenkinsLogParser.Tokens
{
  public class TimestampLine : Token<TimestampLine>, IToken, IHasTimespan
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
      return (IToken)this.MemberwiseClone();
    }

    public bool IsMatchForThisToken(string logLine)
    {
      var tempLogLine = logLine;
      tempLogLine= ReplaceRegularExpression.Replace(tempLogLine, String.Empty);
      var result = RegularExpression.Match(tempLogLine);
      if (result.Success)
      {
        Line = result.Value;
        GenerateTimestampFromLine();
      }
      return result.Success;

    }

    private void GenerateTimestampFromLine()
    {
      var timestampRegEx = new Regex(@"\d+\:\d+\:\d+\.\d{0,3}");
      var result = timestampRegEx.Match(Line);
      if (result.Success)
      {
        var split = result.Value.Split(':');
        if (split.Length > 2)
        {
          var split2 = split[2].Split('.');
          var hours = Convert.ToInt32(split[0]);
          var minutes = Convert.ToInt32(split[1]);
          if (split2.Length > 1)
          {
            var seconds = Convert.ToInt32(split2[0]);
            var milliseconds = TimeHelper.ConvertToMilliseconds(split2[1]);
            Timespan = new TimeSpan(0, hours, minutes, seconds, milliseconds);
          }
          else
          {
            var seconds = Convert.ToInt32(split[2]);
            Timespan = new TimeSpan(0, hours, minutes, seconds);
          }
        }
      }
      else
      {
        Timespan = new TimeSpan(0,0,0,0);
      }
    }

    public bool ProcessLineImpl(string logLine)
    {
      Line = logLine;
      var replacedTestLine = ReplaceRegularExpression.Replace(Line, String.Empty);
      var result = RegularExpression.Match(replacedTestLine);
      if (result.Success)
      {
        Line = result.Value;
        GenerateTimestampFromLine();
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
  }
}
