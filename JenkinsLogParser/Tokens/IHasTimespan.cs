using System;

namespace JenkinsLogParser.Tokens
{
  public interface IHasTimespan
  {
    public TimeSpan GetTimespan();
  }
}