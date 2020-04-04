using System;
using JenkinsLogParser.Events.Projects;
using JenkinsLogParser.Reports;

namespace JenkinsLogParser.Handlers
{
  public class ProjectHandler : IHandles<ProjectStarted>,
                                IHandles<WarningAdded>,
                                IHandles<ProjectEnded>
  {
    private ProjectBuildHierarchyReport _ProjectBuildHierarchyReport;

    public ProjectHandler(ref ProjectBuildHierarchyReport projectBuildHierarchyReport)
    {
      _ProjectBuildHierarchyReport = projectBuildHierarchyReport;
    }

    public void Handle(ProjectStarted tokenEvent)
    {
      var report = _ProjectBuildHierarchyReport;
      var reportArgs = new ProjectBuildHierarchyReportArgs
      {
        LineNumber = tokenEvent.LineNumber,
        ProjectName = tokenEvent.ProjectName,
        Action = ProjectAction.Start
      };
      report.GenerateReportRow(reportArgs);
    }

    public void Handle(ProjectEnded tokenEvent)
    {
      var report = _ProjectBuildHierarchyReport;
      var reportArgs = new ProjectBuildHierarchyReportArgs
      {
        LineNumber = tokenEvent.LineNumber,
        ProjectName = tokenEvent.ProjectName,
        Action = ProjectAction.End
      };
      report.GenerateReportRow(reportArgs);
    }

    public void Handle(WarningAdded tokenEvent)
    {
      
    }
  }
}
