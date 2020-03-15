namespace JenkinsLogParser.Events.Projects
{
  public class ProjectEnded : ITokenEvent
  {
    public string ProjectName { get; set; }
    public ProjectEnded()
    {

    }

    public string GetProject()
    {
      return ProjectName;
    }
  }
}
