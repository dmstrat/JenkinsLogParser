using System;

namespace JenkinsLogParser.Events.Projects
{
  public class ProjectStarted : ITokenEvent
  {
    public long LineNumber { get; set; }
    public string ProjectName { get; set; }

    public ProjectStarted(ProjectStartedEventArgs args)
    {
      LineNumber = args.LineNumber;
      ProjectName = args.ProjectName;
    }

    public ProjectStarted()
    {

    }

    public string GetProject()
    {
      return ProjectName;
    }
  }

  public class ProjectStartedEventArgs : EventArgs
  {
    public long LineNumber { get; set; }
    public string ProjectName { get; set; }
  }
}
