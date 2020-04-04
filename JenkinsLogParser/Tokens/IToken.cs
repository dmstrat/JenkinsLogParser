namespace JenkinsLogParser.Tokens
{
  public interface IToken
  {
    IToken GetClone();
    bool ProcessLine(long lineNumber, string logLine);
    bool PrintIndividualLine();
    string GetLine();
    string GetMatch();
  }
}