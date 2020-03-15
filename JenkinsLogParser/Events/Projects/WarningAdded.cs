namespace JenkinsLogParser.Events.Projects
{
  public class WarningAdded : ITokenEvent
  {
    public string ProjectName { get; set; }
    public string WarningName { get; set; }
    public WarningAdded()
    {

    }

    public string GetProject()
    {
      return ProjectName;
    }
  }
}
