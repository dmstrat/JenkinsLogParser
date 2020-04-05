using System;
using System.Collections.Generic;
using System.Linq;

namespace JenkinsLogParser.Reports
{
  public class ProjectBuildHierarchyReport : Report<ProjectBuildHierarchyReportArgs>
  {
    private static int _Indent;
    private int _LineNumberWidth = 6;
    private IList<string> _ReportRows;
    private IList<ProjectBuildHierarchyReportRow> ReportDataRows;
    public ProjectBuildHierarchyReport() : base()
    {
      _Indent = 0;
      ReportDataRows = new List<ProjectBuildHierarchyReportRow>();
    }

    public override IList<string> GetReportRows()
    {
      _ReportRows = new List<string>();
      BuildPaddingNumbers();
      BuildReportRows();
      return _ReportRows;
    }

    public void BuildPaddingNumbers()
    {
      var maxNumberLength = ReportDataRows.Max(row => row.LineNumber.ToString().Length);
      _LineNumberWidth = maxNumberLength;
    }

    private void BuildReportRows()
    {
      var sortedEventList = ReportDataRows.OrderBy(r => r.LineNumber);
      foreach (var row in sortedEventList)
      {
        var projectAction = BuildActionString(row.Action);
        var indentSpaces = new string(' ', 2 * _Indent);
        var lineNumberRightAligned = row.LineNumber.ToString().PadLeft(_LineNumberWidth);
        var reportRow = $"{lineNumberRightAligned}:{indentSpaces}Project: {row.ProjectName} {projectAction}";
        ModifyIndentBasedOnAction(row.Action);
        _ReportRows.Add(reportRow);
      }
    }

    public override string GenerateReportRow(ProjectBuildHierarchyReportArgs args)
    {
      var reportRow = GenerateReportRowFromArguments(args);
      ReportDataRows.Add(reportRow);
      return string.Empty;
    }

    private ProjectBuildHierarchyReportRow GenerateReportRowFromArguments(ProjectBuildHierarchyReportArgs args)
    {
      var reportRow = new ProjectBuildHierarchyReportRow
      {
        Action = args.Action,
        LineNumber = args.LineNumber,
        ProjectName = args.ProjectName
      };
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

  public class ProjectBuildHierarchyReportRow
  {
    public long LineNumber { get; set; }
    public string ProjectName { get; set; }
    public ProjectAction Action { get; set; }
  }

}
