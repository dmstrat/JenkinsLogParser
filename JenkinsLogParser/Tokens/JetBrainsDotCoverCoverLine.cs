using JenkinsLogParser.Events;
using JenkinsLogParser.Events.DotCover;
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

    private const int DllNameRegexResultGroupIndex = 1;
    private const int TestCategoryRegexResultGroupIndex = 2;

    public JetBrainsDotCoverCoverLine()
    {
      Options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
      var halfSecondTimeout = new TimeSpan(0, 0, 0, 0,500);
      DotCoverWithCategory = new Regex(@"^.*dotCover\.exe.*cover.*\\([^\\]+)\.dll.*TestCategory=(.+[^\""])", Options, halfSecondTimeout);
      DotCoverWithoutCategory = new Regex(@"^.*dotCover\.exe.*cover.*\\([^\\]+)\.dll.*", Options, halfSecondTimeout);
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
        var result2 = DotCoverWithoutCategory.Match(tempLogLine);
        var foundRowWithCategory = result.Success && result2.Success;
        var foundRowWithoutCategory = !result.Success && result2.Success;

        
        if (foundRowWithCategory)
        {
          var category = result.Groups[TestCategoryRegexResultGroupIndex].Value;
          var dll = result.Groups[DllNameRegexResultGroupIndex].Value;
          var combined = $"{dll}-{category}";
          //Line = result.Groups[TestCategoryRegexResultGroupIndex].Value;
          RaiseDotCoverCoverExecutionWithCategoryEvent(lineNumber, combined, logLine);
        }

        if (foundRowWithoutCategory)
        {
          var dll = result2.Groups[DllNameRegexResultGroupIndex].Value;
          var combined = $"{dll}-{dll}";
          RaiseDotCoverCoverExecutionEvent(lineNumber, combined, logLine);
        }
        /*
        if (result.Success)
        {
          Line = result.Groups[TestCategoryRegexResultGroupIndex].Value;
          RaiseDotCoverCoverExecutionEvent(lineNumber, Line, logLine);
        } */
        return foundRowWithoutCategory || foundRowWithCategory;
      }
      catch (RegexMatchTimeoutException)
      {
        return false;
      }
    }

    private void RaiseDotCoverCoverExecutionWithCategoryEvent(long lineNumber, string line, string fullText)
    {
      var args = BuildDotCoverCoverEventArgs(lineNumber, line, fullText);
      var dotCoverCoverAdded = new DotCoverCoverExecutionAdded(args);
      TokenEvents.Raise(dotCoverCoverAdded);
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
