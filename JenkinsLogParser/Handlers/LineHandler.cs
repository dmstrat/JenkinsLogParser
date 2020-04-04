using System;
using JenkinsLogParser.Events;

namespace JenkinsLogParser.Handlers
{
  public class LineHandler : IHandles<LineAdded>
  {
    public LineHandler()
    {
    }

    public void Handle(LineAdded tokenEvent)
    {

    }
  }
}
