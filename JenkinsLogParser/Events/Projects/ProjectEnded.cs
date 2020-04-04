namespace JenkinsLogParser.Events.Projects
{
  public class ProjectEnded : EventBase
  {
    public string ProjectName => RegExResult;

    public ProjectEnded(ProjectEndedEventArgs args): base(args)
    {
    }
  }

  public class ProjectEndedEventArgs : EventArgsBase
  {
    public string ProjectName { get; set; }
  }
}
