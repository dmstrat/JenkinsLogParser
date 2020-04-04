namespace JenkinsLogParser.Events
{
  public class TimestampAdded : EventBase
  {
    public TimestampAdded(TimestampAddedEventArgs args) : base(args)
    {
    }
  }

  public class TimestampAddedEventArgs : EventArgsBase
  {
  }

}
