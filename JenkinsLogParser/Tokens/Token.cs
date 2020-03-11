using System;
using System.Collections.Generic;
using System.Text;

namespace JenkinsLogParser.Tokens
{
  public class Token<T> : Token where T : class, IToken, new()
  {
    public static T ProcessLine(string logLine)
    {
      var newToken = new T();
      var lineIsThisToken = newToken.ProcessLineImpl(logLine);
      if (lineIsThisToken)
      {
        return newToken;
      }
      return null;
    }
  }

  public abstract class Token
  {
    public string GetTokenName()
    {
      return "GetTokenName";
    }
  }

}
