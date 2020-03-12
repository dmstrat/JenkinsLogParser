namespace JenkinsLogParser.Tokens
{
  public interface IToken
  {
    IToken GetClone();
    bool IsMatchForThisToken(string logLine);
    string GetLine();
  }
}