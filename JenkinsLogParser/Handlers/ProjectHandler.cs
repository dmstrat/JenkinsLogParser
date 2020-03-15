using JenkinsLogParser.Events.Projects;

namespace JenkinsLogParser.Handlers
{
  public class ProjectHandler : IHandles<ProjectStarted>, 
                                IHandles<ProjectEnded>
  {
    public ProjectHandler()
    {
    }

    public void Handle(ProjectStarted tokenEvent)
    {

    }

    public void Handle(ProjectEnded tokenEvent)
    {

    }
  }
}
