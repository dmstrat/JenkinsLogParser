using System;

namespace JenkinsLogParser.Events.Projects
{
  public class ProjectEnded : ITokenEvent
  {
    public long LineNumber { get; set; }
    public string ProjectName { get; set; }

    public ProjectEnded(ProjectEndedEventArgs args)
    {
      LineNumber = args.LineNumber;
      ProjectName = args.ProjectName;
    }

    public ProjectEnded()
    {

    }

    public string GetProject()
    {
      return ProjectName;
    }
  }

  public class ProjectEndedEventArgs : EventArgs
  {
    public long LineNumber { get; set; }
    public string ProjectName { get; set; }
  }
}
