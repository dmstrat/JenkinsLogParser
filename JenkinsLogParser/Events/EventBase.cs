using System;

namespace JenkinsLogParser.Events
{
  public class EventBase : ITokenEvent
  {
    
    public EventBase(EventArgsBase args)
    {
      LineNumber = args.LineNumber;
      FullText = args.FullText;
      RegExResult = args.RegExResult;
    }

    public long LineNumber { get; set; }
    public string FullText { get; set; }
    public string RegExResult { get; set; }
  }

  public class EventArgsBase : EventArgs
  {
    public long LineNumber { get; set; }
    public string FullText { get; set; }
    public string RegExResult { get; set; }
  }
}
