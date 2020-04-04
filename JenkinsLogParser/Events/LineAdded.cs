namespace JenkinsLogParser.Events
{
  public class LineAdded : EventBase
  {
    public LineAdded(LineAddedEventArgs args) : base(args)
    {
    }
  }

  public class LineAddedEventArgs : EventArgsBase
  {
  }

}
