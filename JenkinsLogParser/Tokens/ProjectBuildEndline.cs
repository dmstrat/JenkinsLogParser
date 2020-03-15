﻿using System;
using System.Text.RegularExpressions;

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

    public string GetMatch()
    {
      return Line;
    }
  }
}
