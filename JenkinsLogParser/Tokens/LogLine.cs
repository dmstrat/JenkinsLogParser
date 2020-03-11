using System.Text.RegularExpressions;

namespace JenkinsLogParser.Tokens
{
  public abstract class LogLine
  {
    public string Line { get; set; }
    public Regex RegularExpression { get; set; }
    public Regex ReplaceRegularExpression { get; set; }

    public abstract bool ProcessLine(string logLine);
  }
}
