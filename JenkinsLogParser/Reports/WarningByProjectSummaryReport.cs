using System.Collections.Generic;
using System.Linq;

namespace JenkinsLogParser.Reports
{
  public class WarningByProjectSummaryReport  : Report<WarningByProjectSummaryReportArgs>
  {
    private const string EXTERNAL = "EXTERNAL";
    private IList<string> _ReportRows;
    private Dictionary<string, Dictionary<string, int>> _ReportDataRows;
    private int _CountPadding = 5;
    private int _WarningPadding = 12;

    public WarningByProjectSummaryReport() : base()
    {
      _ReportRows = new List<string>();
      _ReportDataRows = new Dictionary<string, Dictionary<string, int>>();
    }

    public override string GetReportName()
    {
      return "Warnings By Projects Summary";
    }

    public override string GetReportRowHeaders()
    {
      return "Project: [ProjectName] \r\n  Warning:[WarningName]:[Count]";
    }

    public override IList<string> GetReportRows()
    {
      _ReportRows = new List<string>();
      BuildPaddingNumbers();
      BuildReportRows();
      return _ReportRows;
    }

    public override void AddDataRow(WarningByProjectSummaryReportArgs args)
    {
      EnsureProjectInDictionary(args.ProjectName);
      EnsureWarningInProjectInDictionary(args.ProjectName, args.WarningName);
      IncrementWarningCountInProject(args.ProjectName, args.WarningName);
    }

    private void BuildPaddingNumbers()
    {
      var maxWarningStringLength = _ReportDataRows.Max(row => row.Value.Keys.Max(ro=>ro.Length));
      _WarningPadding = maxWarningStringLength;
      var maxCountStringLength = _ReportDataRows.Max(row => row.Value.Values.Max(ro => ro.ToString().Length));
      _CountPadding = maxCountStringLength;
    }

    private void EnsureProjectInDictionary(string projectName)
    {
      var projectNotInDictionary = !_ReportDataRows.ContainsKey(projectName);
      if (projectNotInDictionary)
      {
        var newWarningDictionary = new Dictionary<string, int>();
        _ReportDataRows.Add(projectName, newWarningDictionary);
      }
    }

    private void EnsureWarningInProjectInDictionary(string projectName, string warningName)
    {
      var warningNotInProjectDictionary = !_ReportDataRows[projectName].ContainsKey(warningName);
      if (warningNotInProjectDictionary)
      {
        _ReportDataRows[projectName].Add(warningName, 0);
      }
    }

    private void IncrementWarningCountInProject(string projectName, string warningName)
    {
      _ReportDataRows[projectName][warningName]++;
    }

    private void BuildReportsRowsUsingProjectList(IList<string> sortedProjectList)
    {
      foreach (var projectName in sortedProjectList)
      {
        var projectHeaderRow = $"Project: {projectName}";
        _ReportRows.Add(projectHeaderRow);
        var sortedWarningsList = _ReportDataRows[projectName].Keys.ToList();
        sortedWarningsList.Sort();
        foreach (var warningName in sortedWarningsList)
        {
          var warningCount = _ReportDataRows[projectName][warningName];
          var warningCountRightAligned = warningCount.ToString().PadLeft(_CountPadding);
          var warningNameLeftAligned = warningName.PadRight(_WarningPadding);
          var reportRow = $"  Warning: {warningNameLeftAligned}:{warningCountRightAligned}";
          _ReportRows.Add(reportRow);
        }
      }
    }

    private void BuildReportRows()
    {
      //extract EXTERNAL project to print first 
      var externalProjectList = _ReportDataRows.Keys.Where(rpt => rpt.Equals(EXTERNAL)).ToList();
      BuildReportsRowsUsingProjectList(externalProjectList);

      //extract ALL BUT EXTERNAL project to print next
      var sortedProjectList = _ReportDataRows.Keys.Where(rpt=>!rpt.Equals(EXTERNAL)).ToList();
      sortedProjectList.Sort();
      BuildReportsRowsUsingProjectList(sortedProjectList);
    }
  }

  public class WarningByProjectSummaryReportArgs : ReportArgs
  {
    public string ProjectName { get; set; }
    public string WarningName { get; set; }
  }
}
