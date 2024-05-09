using JenkinsLogParser.Events;
using JenkinsLogParser.Events.Projects;
using System;
using System.Text.RegularExpressions;

namespace JenkinsLogParser.Tokens
{
  public class JetBrainsDotCoverCoverLine : IToken 
  {
    public Regex DotCoverWithoutCategory { get; set; }
    public Regex DotCoverWithCategory { get; set; }
    public Regex ReplaceRegularExpression { get; set; }
    public string Line { get; set; }
    public RegexOptions Options { get; set; }

    private const int TestCategoryRegexResultGroupIndex = 1;

    public JetBrainsDotCoverCoverLine()
    {
      Options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
      var halfSecondTimeout = new TimeSpan(0, 0, 0, 0,500);
      DotCoverWithCategory = new Regex(@"^.*dotCover\.exe.*cover.*TestCategory=(.+[^\""])", Options, halfSecondTimeout);
      DotCoverWithoutCategory = new Regex(@"^.*dotCover\.exe.*cover.*", Options, halfSecondTimeout);
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
        var result = DotCoverWithCategory.Match(tempLogLine);
        if (result.Success)
        {
          Line = result.Groups[TestCategoryRegexResultGroupIndex].Value;
          RaiseDotCoverCoverExecutionEvent(lineNumber, Line, logLine);
        }
        return result.Success;
      }
      catch (RegexMatchTimeoutException)
      {
        return false;
      }
    }

    private void RaiseDotCoverCoverExecutionEvent(long lineNumber, string line, string fullText)
    {
      var args = BuildDotCoverCoverEventArgs(lineNumber, line, fullText);
      var dotCoverCoverAdded = new DotCoverCoverExecutionAdded(args);
      TokenEvents.Raise(dotCoverCoverAdded);
    }

    private DotCoverCoverEventArgs BuildDotCoverCoverEventArgs(long lineNumber, string line, string fullText)
    {
      var dotCoverCoverEventArgs = new DotCoverCoverEventArgs()
      {
        LineNumber = lineNumber,
        TestCategory = line,
        RegExResult = line, 
        FullText = fullText,
      };
      return dotCoverCoverEventArgs;
    }
  }
}
