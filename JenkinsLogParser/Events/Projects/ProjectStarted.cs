namespace JenkinsLogParser.Events.Projects
{
  public class ProjectStarted : ITokenEvent
  {
    public string ProjectName { get; set; }
    public ProjectStarted()
    {

    }

    public string GetProject()
    {
      return ProjectName;
    }
  }
}
