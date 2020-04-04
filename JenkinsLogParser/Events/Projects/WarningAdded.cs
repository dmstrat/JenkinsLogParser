namespace JenkinsLogParser.Events.Projects
{
  public class WarningAdded : EventBase
  {
    public string ProjectName { get; set; }
    public string WarningName { get; set; }

    public WarningAdded(WarningAddedEventArgs args): base(args)
    {
      ProjectName = args.ProjectName;
      WarningName = args.WarningName;
    }
  }

  public class WarningAddedEventArgs : EventArgsBase
  {
    public string WarningName { get; set; }
    public string ProjectName { get; set; }
  }
}
