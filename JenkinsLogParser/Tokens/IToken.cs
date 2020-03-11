namespace JenkinsLogParser.Tokens
{
  public interface IToken
  {
    bool ProcessLineImpl(string logLine);
    string GetLine();
  }
}