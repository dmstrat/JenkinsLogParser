namespace JenkinsLogParser.Events.Projects
{
  public class WarningAdded : EventBase
  {
    public string WarningName { get; set; }

    public WarningAdded(WarningAddedEventArgs args): base(args)
    {
      WarningName = args.WarningName;
    }
  }

  public class WarningAddedEventArgs : EventArgsBase
  {
    public string WarningName { get; set; }
  }
}
