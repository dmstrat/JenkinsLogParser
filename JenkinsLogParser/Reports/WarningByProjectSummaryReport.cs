using JenkinsLogParser.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace JenkinsLogParser.Reports
{
  public class WarningByProjectSummaryReport  : Report<WarningByProjectSummaryReportArgs>
  {
    private const string EXTERNAL = "EXTERNAL";
    private IList<string> _ReportRows;
    private Dictionary<string, Dictionary<string, int>> _ReportDataRows;
    private Dictionary<string, string> _ReportWarningSampleText;
    private int _CountPadding = 5;
    private int _WarningPadding = 12;

    public WarningByProjectSummaryReport() : base()
    {
      _ReportRows = new List<string>();
      _ReportDataRows = new Dictionary<string, Dictionary<string, int>>();
      _ReportWarningSampleText = new Dictionary<string, string>();
    }

    public override string GetReportName()
    {
      return "Warnings By Projects Summary";
    }

    public override string GetReportRowHeaders()
    {
      return "Project: [ProjectName] \r\n  Warning:[WarningName]:[Count]:[Description or Sample Entry]";
    }

    public override IList<string> GetReportRows()
    {
      InitializeReport();
      //build header 
      BuildReportRows();
      //build footer
      return _ReportRows;
    }

    private void InitializeReport()
    {
      _ReportRows = new List<string>();
      UpdateWarningsDictionaryWithPreDefinedDescriptions();
      BuildPaddingNumbers();
    }

    private void UpdateWarningsDictionaryWithPreDefinedDescriptions()
    {
      var warningDefinitions = WarningDefinitionsLoader.LoadWarnings();
      foreach (var warningDefinition in warningDefinitions)
      {
        var needToOverrideDescription = _ReportWarningSampleText.ContainsKey(warningDefinition.Key);
        if(needToOverrideDescription)
        {
          _ReportWarningSampleText[warningDefinition.Key] = warningDefinition.Value;
        }
        else
        {
          _ReportWarningSampleText.Add(warningDefinition.Key,warningDefinition.Value);
        }
      }
    }

    public override void AddDataRow(WarningByProjectSummaryReportArgs args)
    {
      EnsureProjectInDictionary(args.ProjectName);
      EnsureWarningInProjectInDictionary(args.ProjectName, args.WarningName, args.WarningDefinition);
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

    private void EnsureWarningInProjectInDictionary(string projectName, string warningName, string sampleWarningText)
    {
      var warningNotInProjectDictionary = !_ReportDataRows[projectName].ContainsKey(warningName);
      if (warningNotInProjectDictionary)
      {
        _ReportDataRows[projectName].Add(warningName, 0);
        EnsureWarningInSampleDictionary(warningName, sampleWarningText);
      }
    }

    private void EnsureWarningInSampleDictionary(string warningName, string sampleWarningText)
    {
      var warningNotYetCached = !_ReportWarningSampleText.ContainsKey(warningName);
      if (warningNotYetCached)
      {
        _ReportWarningSampleText.Add(warningName, sampleWarningText);
      }
    }

    private void IncrementWarningCountInProject(string projectName, string warningName)
    {
      _ReportDataRows[projectName][warningName]++;
    }

    private void BuildReportsRowsUsingProjectListByCountDescending(IEnumerable<string> sortedProjectList)
    {
      foreach (var projectName in sortedProjectList)
      {
        var projectHeaderRow = $"Project: {projectName}";
        _ReportRows.Add(projectHeaderRow);
        var sortedWarningsList = _ReportDataRows[projectName].OrderByDescending(x=>x.Value);//.Keys.ToList();

        foreach (var warning in sortedWarningsList)
        {
          var warningCount = _ReportDataRows[projectName][warning.Key];
          var warningCountRightAligned = warningCount.ToString().PadLeft(_CountPadding);
          var warningNameLeftAligned = warning.Key.PadRight(_WarningPadding);
          var warningSampleText = _ReportWarningSampleText[warning.Key];
          var reportRow = $"  Warning: {warningNameLeftAligned}:{warningCountRightAligned}:{warningSampleText}";
          _ReportRows.Add(reportRow);
        }
      }
    }

    private void BuildReportRows()
    {
      //extract EXTERNAL project to print first 
      var externalProjectList = _ReportDataRows.Keys.Where(rpt => rpt.Equals(EXTERNAL)).ToList();
      //BuildReportsRowsUsingProjectList(externalProjectList);
      BuildReportsRowsUsingProjectListByCountDescending(externalProjectList);

      //extract ALL BUT EXTERNAL project to print next
      var sortedProjectList = _ReportDataRows.Keys.Where(rpt=>!rpt.Equals(EXTERNAL)).ToList();
      sortedProjectList.Sort();
      //BuildReportsRowsUsingProjectList(sortedProjectList);
      BuildReportsRowsUsingProjectListByCountDescending(sortedProjectList);
    }
  }

  public class WarningByProjectSummaryReportArgs : ReportArgs
  {
    public string ProjectName { get; set; }
    public string WarningName { get; set; }
    public string WarningDefinition { get; set; }
  }
}
