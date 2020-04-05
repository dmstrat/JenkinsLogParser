using System.Collections.Generic;
using System.Linq;

namespace JenkinsLogParser.Reports
{
  public class WarningByProjectSummaryReport  : Report<WarningByProjectSummaryReportArgs>
  {
    private const string EXTERNAL = "EXTERNAL";
    private IList<string> ReportRows;
    private Dictionary<string, Dictionary<string, int>> ReportDataRows;
    public WarningByProjectSummaryReport() : base()
    {
      ReportRows = new List<string>();
      ReportDataRows = new Dictionary<string, Dictionary<string, int>>();
    }

    public override IList<string> GetReportRows()
    {
      ReportRows = new List<string>();
      BuildReportRows();
      return ReportRows;
    }

    public override string GenerateReportRow(WarningByProjectSummaryReportArgs args)
    {
      EnsureProjectInDictionary(args.ProjectName);
      EnsureWarningInProjectInDictionary(args.ProjectName, args.WarningName);
      IncrementWarningCountInProject(args.ProjectName, args.WarningName);
      return string.Empty;
    }

    private void EnsureProjectInDictionary(string projectName)
    {
      var projectNotInDictionary = !ReportDataRows.ContainsKey(projectName);
      if (projectNotInDictionary)
      {
        var newWarningDictionary = new Dictionary<string, int>();
        ReportDataRows.Add(projectName, newWarningDictionary);
      }
    }

    private void EnsureWarningInProjectInDictionary(string projectName, string warningName)
    {
      var warningNotInProjectDictionary = !ReportDataRows[projectName].ContainsKey(warningName);
      if (warningNotInProjectDictionary)
      {
        ReportDataRows[projectName].Add(warningName, 0);
      }
    }

    private void IncrementWarningCountInProject(string projectName, string warningName)
    {
      ReportDataRows[projectName][warningName]++;
    }

    private void BuildReportsRowsUsingProjectList(IList<string> sortedProjectList)
    {
      foreach (var projectName in sortedProjectList)
      {
        var projectHeaderRow = $"Project: {projectName}";
        ReportRows.Add(projectHeaderRow);
        var sortedWarningsList = ReportDataRows[projectName].Keys.ToList();
        sortedWarningsList.Sort();
        foreach (var warningName in sortedWarningsList)
        {
          var warningCount = ReportDataRows[projectName][warningName];
          var reportRow = $"  Warning: {warningName}:{warningCount}";
          ReportRows.Add(reportRow);
        }
      }
    }

    private void BuildReportRows()
    {
      //extract EXTERNAL project to print first 
      var externalProjectList = ReportDataRows.Keys.Where(rpt => rpt.Equals(EXTERNAL)).ToList();
      BuildReportsRowsUsingProjectList(externalProjectList);

      //extract ALL BUT EXTERNAL project to print next
      var sortedProjectList = ReportDataRows.Keys.Where(rpt=>!rpt.Equals(EXTERNAL)).ToList();
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
