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
    //internal static WarningDictionary ProjectWarningCount;
    internal WarningSummaryReport WarningSummaryReport;

    public WarningHandler(ref WarningSummaryReport warningSummary)
    {
      WarningSummaryReport = warningSummary;
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
      var report = WarningSummaryReport;
      var reportArgs = GenerateReportArgs(tokenEvent);
      report.GenerateReportRow(reportArgs);
    }

    private WarningSummaryReportArgs GenerateReportArgs(WarningAdded warningAddedEvent)
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
