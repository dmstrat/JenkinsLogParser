namespace JenkinsLogParser.Events.Projects
{
  public class ProjectStarted : EventBase
  {
    public string ProjectName => RegExResult;

    public ProjectStarted(ProjectStartedEventArgs args): base(args)
    {
      RegExResult = args.ProjectName;
    }
  }

  public class ProjectStartedEventArgs : EventArgsBase
  {
    public string ProjectName { get; set; }
  }
}
