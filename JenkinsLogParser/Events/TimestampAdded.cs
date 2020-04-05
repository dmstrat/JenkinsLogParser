using System;

namespace JenkinsLogParser.Events
{
  public class TimestampAdded : EventBase
  {
    public TimeSpan TimeSpan { get; set; }
    public TimestampAdded(TimestampAddedEventArgs args) : base(args)
    {
      TimeSpan = args.TimeSpan;
    }
  }

  public class TimestampAddedEventArgs : EventArgsBase
  {
    public TimeSpan TimeSpan { get; set; }
  }

}
