using JenkinsLogParser.Events;

namespace JenkinsLogParser
{
  public interface IHandles<T> where T : ITokenEvent
  {
    void Handle(T tokenEvent);
  }
}
