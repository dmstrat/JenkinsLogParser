namespace JenkinsLogParser.Events.Projects
{
  public class ProjectStarted : EventBase
  {
    public string ProjectName { get; set; }

    public ProjectStarted(ProjectStartedEventArgs args): base(args)
    {
      ProjectName = args.ProjectName;
    }
  }

  public class ProjectStartedEventArgs : EventArgsBase
  {
    public string ProjectName { get; set; }
  }
}
