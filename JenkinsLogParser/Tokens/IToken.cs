namespace JenkinsLogParser.Tokens
{
  public interface IToken
  {
    bool ProcessLine(long lineNumber, string logLine);
  }
}