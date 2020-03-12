namespace JenkinsLogParser.Tokens
{
  public interface IToken
  {
    IToken GetClone();
    bool IsMatchForThisToken(string logLine);
    bool ProcessLineImpl(string logLine);
    string GetLine();
  }
}