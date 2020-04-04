using System;

namespace JenkinsLogParser.Reports
{
  public class ProjectBuildHierarchyReport : Report<ProjectBuildHierarchyReportArgs>
  {
    private static int _Indent;
    private const int LineNumberWidth = 6;
    public ProjectBuildHierarchyReport() : base()
    {
      _Indent = 0;
    }

    public override string GenerateReportRow(ProjectBuildHierarchyReportArgs args)
    {
      var reportRow = GenerateReportRowFromArguments(args);
      base.AddRow(reportRow);
      return reportRow;
    }

    private string GenerateReportRowFromArguments(ProjectBuildHierarchyReportArgs args)
    {
      var projectAction = BuildActionString(args.Action);
      var indentSpaces = new string(' ', 2 * _Indent);
      var lineNumberRightAligned = args.LineNumber.ToString().PadLeft(LineNumberWidth);
      var reportRow = $"{lineNumberRightAligned}:{indentSpaces}Project: {args.ProjectName} {projectAction}";
      ModifyIndentBasedOnAction(args.Action);
      return reportRow;
    }

    private void ModifyIndentBasedOnAction(ProjectAction projectAction)
    {
      switch (projectAction)
      {
        case ProjectAction.Start:
          _Indent++;
          break;
        case ProjectAction.End:
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(projectAction), projectAction, null);
      }
    }

    private string BuildActionString(ProjectAction projectAction)
    {
      switch (projectAction)
      {
        case ProjectAction.Start:
          return "START";
        case ProjectAction.End:
          _Indent--;
          return "END";
        default:
          throw new ArgumentOutOfRangeException(nameof(projectAction), projectAction, null);
      }
    }
  }

  public class ProjectBuildHierarchyReportArgs : ReportArgs
  {
    public long LineNumber { get; set; }
    public string ProjectName { get; set; }
    public ProjectAction Action { get; set; }
  }

  public enum ProjectAction
  {
    Start,
    End
  }
}
