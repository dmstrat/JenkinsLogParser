using JenkinsLogParser.Events;
using JenkinsLogParser.Events.Projects;
using System;
using System.Collections.Generic;

namespace JenkinsLogParser.Unit.Tests.Helpers
{
  public class TestDelegateHelper
  {
    public IList<Delegate> BuildTestDelegatesDictionary()
    {
      var newDelegates = new List<Delegate>();

      var testEventHandler = new TestEventHandler();
      TokenEvents.Register<ProjectStarted>(testEventHandler.Handle);
      TokenEvents.Register<ProjectEnded>(testEventHandler.Handle);
      TokenEvents.Register<WarningAdded>(testEventHandler.Handle);
      TokenEvents.Register<TimestampAdded>(testEventHandler.Handle);
      newDelegates.Add((Action<ProjectStarted>)testEventHandler.Handle);
      newDelegates.Add((Action<ProjectEnded>)testEventHandler.Handle);
      newDelegates.Add((Action<WarningAdded>)testEventHandler.Handle);
      newDelegates.Add((Action<TimestampAdded>)testEventHandler.Handle);

      return newDelegates;
    }
  }
}
