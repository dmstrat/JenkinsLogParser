namespace JenkinsLogParser.Events.Projects
{
  public class DotCoverCoverExecutionAdded : EventBase
  {
    public string TestCategory => RegExResult;

    public DotCoverCoverExecutionAdded(DotCoverCoverEventArgs args): base(args)
    {
      RegExResult = args.TestCategory;
    }
  }

  public class DotCoverCoverEventArgs : EventArgsBase
  {
    public string TestCategory { get; set; }
  }
}
