using JenkinsLogParser.Events;
using System.Collections.Generic;
using System.Linq;

namespace JenkinsLogParser.Unit.Tests.Helpers
{
  public static class EventHelper
  {
    public static IEnumerable<TestEventInformation> GetEventsByType(IList<TestEventInformation> events, string eventName)
    {
      var foundEvents = events.Where(e => e.EventName == eventName);
      return foundEvents;
    }

    private static bool IsMatchBy(IEnumerable<TestEventInformation> events, long lineNumber, string fullText, string regexText)
    {
      var returnValue = false;
      foreach (var eventInformation in events)
      {
        var tokenEvent = (EventBase)eventInformation.TokenEvent;
        var lineNumberMatch = tokenEvent.LineNumber == lineNumber;
        if (lineNumberMatch)
        {
          var fullTextMatch = tokenEvent.FullText == fullText;
          var regexMatch = tokenEvent.RegExResult == regexText;
          if (fullTextMatch && regexMatch)
          {
            return true;
          }
        }
      }
      return returnValue;
    }

    public static bool HasSpecificProjectStartEvent(long lineNumber, string fullText, string regexText)
    {
      var eventName = "ProjectStarted";
      var events = TestEventHandler.GetEvents();
      var foundEventsOfType = GetEventsByType(events, eventName);
      return IsMatchBy(foundEventsOfType, lineNumber, fullText, regexText);
    }

    public static bool HasSpecificProjectEndedEvent(long lineNumber, string fullText, string regexText)
    {
      var eventName = "ProjectEnded";
      var events = TestEventHandler.GetEvents();
      var foundEventsOfType = GetEventsByType(events, eventName);
      return IsMatchBy(foundEventsOfType, lineNumber, fullText, regexText);
    }
    
    public static bool HasSpecificWarningAddedEvent(long lineNumber, string fullText, string regexText)
    {
      var eventName = "WarningAdded";
      var events = TestEventHandler.GetEvents();
      var foundEventsOfType = GetEventsByType(events, eventName);
      return IsMatchBy(foundEventsOfType, lineNumber, fullText, regexText);
    }

    public static bool HasSpecificTimestampAddedEvent(long lineNumber, string fullText, string regexText)
    {
      var eventName = "TimestampAdded";
      var events = TestEventHandler.GetEvents();
      var foundEventsOfType = GetEventsByType(events, eventName);
      return IsMatchBy(foundEventsOfType, lineNumber, fullText, regexText);
    }
  }
}
