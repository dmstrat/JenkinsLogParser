using JenkinsLogParser.Events.Projects;
using JenkinsLogParser.Reports;

namespace JenkinsLogParser.Handlers
{
  public class ProjectHandler : IHandles<ProjectStarted>,
                                IHandles<ProjectEnded>
  {
    private readonly ProjectBuildHierarchyReport _ProjectBuildHierarchyReport;

    public ProjectHandler(ref ProjectBuildHierarchyReport projectBuildHierarchyReport)
    {
      _ProjectBuildHierarchyReport = projectBuildHierarchyReport;
    }

    public void Handle(ProjectStarted tokenEvent)
    {
      var reportArgs = new ProjectBuildHierarchyReportArgs
      {
        LineNumber = tokenEvent.LineNumber,
        ProjectName = tokenEvent.ProjectName,
        Action = ProjectAction.Start
      };
      _ProjectBuildHierarchyReport.AddDataRow(reportArgs);
    }

    public void Handle(ProjectEnded tokenEvent)
    {
      var reportArgs = new ProjectBuildHierarchyReportArgs
      {
        LineNumber = tokenEvent.LineNumber,
        ProjectName = tokenEvent.ProjectName,
        Action = ProjectAction.End
      };
      _ProjectBuildHierarchyReport.AddDataRow(reportArgs);
    }
  }
}
