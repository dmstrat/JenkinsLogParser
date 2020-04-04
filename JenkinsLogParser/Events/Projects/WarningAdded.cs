using System;

namespace JenkinsLogParser.Events.Projects
{
  public class WarningAdded : ITokenEvent
  {
    public long LineNumber { get; set; }
    public string ProjectName { get; set; }
    public string WarningName { get; set; }

    public WarningAdded(WarningAddedEventArgs args)
    {
      LineNumber = args.LineNumber;
      ProjectName = args.ProjectName;
      WarningName = args.WarningName;
    }

    public string GetProject()
    {
      return ProjectName;
    }
  }

  public class WarningAddedEventArgs : EventArgs
  {
    public long LineNumber { get; set; }
    public string WarningName { get; set; }
    public string ProjectName { get; set; }
  }
}
