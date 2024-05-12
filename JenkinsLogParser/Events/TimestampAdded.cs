using System;

namespace JenkinsLogParser.Events
{
  public class TimestampAdded(TimestampAddedEventArgs args) : EventBase(args)
  {
    public TimeSpan TimeSpan { get; set; } = args.TimeSpan;
  }

  public class TimestampAddedEventArgs : EventArgsBase
  {
    public TimeSpan TimeSpan { get; set; }
  }
}
