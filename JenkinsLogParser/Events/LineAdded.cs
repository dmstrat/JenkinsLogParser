using System;
using JenkinsLogParser.Handlers;

namespace JenkinsLogParser.Events
{
  public class LineAdded : ITokenEvent
  {
    private long _LineNumber;
    private string _Line; 

    public LineAdded(LineAddedEventArgs args)
    {
      _LineNumber = args.LineNumber;
      _Line = args.LogLine;
    }

    public string GetProject()
    {
      return String.Empty;
    }
  }

  public class LineAddedEventArgs : EventArgs
  {
    public long LineNumber { get; set; }
    public string LogLine { get; set; }
  }

}
