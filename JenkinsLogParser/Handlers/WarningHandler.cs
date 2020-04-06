using JenkinsLogParser.Events.Projects;
using System.Collections.Generic;
using JenkinsLogParser.Reports;

namespace JenkinsLogParser.Handlers
{
  public class WarningHandler : IHandles<ProjectStarted>,
                                IHandles<WarningAdded>,
                                IHandles<ProjectEnded>
  {
    private static Stack<string> _ProjectStack;
    internal WarningSummaryReport WarningSummaryReport;
    internal WarningByProjectSummaryReport WarningByProjectSummaryReport;
    private const string EXTERNAL = "EXTERNAL";

    internal string CurrentProject
    {
      get
      {
        var stackHasProject = _ProjectStack.Count > 0;
        if (stackHasProject)
        {
          return _ProjectStack.Peek();
        }
        return EXTERNAL;
      }
    }

    public WarningHandler(ref WarningSummaryReport warningSummary, ref WarningByProjectSummaryReport warningByProjectSummaryReport)
    {
      WarningSummaryReport = warningSummary;
      WarningByProjectSummaryReport = warningByProjectSummaryReport;
      _ProjectStack = new Stack<string>();
    }

    public void Handle(ProjectStarted tokenEvent)
    {
      AddProjectToStack(tokenEvent.ProjectName);
    }

    public void Handle(ProjectEnded tokenEvent)
    {
      PopFromStack();
    }

    private void PopFromStack()
    {
      _ProjectStack.Pop();
    }

    public void Handle(WarningAdded tokenEvent)
    {
      AddWarningToWarningSummaryReport(tokenEvent);
      AddWarningToWarningByProjectSummaryReport(tokenEvent);
    }

    private void AddWarningToWarningSummaryReport(WarningAdded warningAddedEvent)
    {
      var report = WarningSummaryReport;
      var reportArgs = GenerateWarningSummaryReportArgs(warningAddedEvent);
      report.AddDataRow(reportArgs);
    }

    private void AddWarningToWarningByProjectSummaryReport(WarningAdded warningAddedEvent)
    {
      var report = WarningByProjectSummaryReport;
      var reportArgs = GenerateWarningByProjectSummaryReportArgs(warningAddedEvent);
      report.AddDataRow(reportArgs);
    }

    private WarningByProjectSummaryReportArgs GenerateWarningByProjectSummaryReportArgs(WarningAdded warningAddedEvent)
    {
      var reportArgs = new WarningByProjectSummaryReportArgs
      {
        ProjectName = CurrentProject,
        WarningName = warningAddedEvent.WarningName
      };
      return reportArgs;
    }

    private WarningSummaryReportArgs GenerateWarningSummaryReportArgs(WarningAdded warningAddedEvent)
    {
      var reportArgs = new WarningSummaryReportArgs
      {
        WarningName = warningAddedEvent.WarningName
      };
      return reportArgs;
    }

    private void AddProjectToStack(string projectName)
    {
      _ProjectStack.Push(projectName);
    }
  }
}
