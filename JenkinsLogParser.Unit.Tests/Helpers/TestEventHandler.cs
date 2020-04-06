using JenkinsLogParser.Events;
using JenkinsLogParser.Events.Projects;
using System;
using System.Collections.Generic;

namespace JenkinsLogParser.Unit.Tests.Helpers
{
  public class TestEventHandler : IHandles<ProjectStarted>,
                                  IHandles<ProjectEnded>, 
                                  IHandles<WarningAdded>, 
                                  IHandles<TimestampAdded>
  {
    private static IList<TestEventInformation> _Events;

    public static IList<TestEventInformation> GetEvents()
    {
      return _Events;
    }

    public TestEventHandler()
    {
      _Events = new List<TestEventInformation>();
    }
    public void Handle(ProjectStarted tokenEvent)
    {
      var newEventInfo = new TestEventInformation
      {
        EventName = "ProjectStarted",
        TokenEvent = tokenEvent
      };
      _Events.Add(newEventInfo);
    }

    public void Handle(ProjectEnded tokenEvent)
    {
      var newEventInfo = new TestEventInformation
      {
        EventName = "ProjectEnded",
        TokenEvent = tokenEvent
      };
      _Events.Add(newEventInfo);
    }

    public void Handle(WarningAdded tokenEvent)
    {
      var newEventInfo = new TestEventInformation
      {
        EventName = "WarningAdded",
        TokenEvent = tokenEvent
      };
      _Events.Add(newEventInfo);
    }

    public void Handle(TimestampAdded tokenEvent)
    {
      var newEventInfo = new TestEventInformation
      {
        EventName = "TimestampAdded",
        TokenEvent = tokenEvent
      };
      _Events.Add(newEventInfo);
    }
  }

  public class TestEventInformation
  {
    public string EventName { get; set; }
    public object TokenEvent { get; set; }
  }
}
