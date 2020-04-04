using System.Collections.Generic;
using System.Linq;

namespace JenkinsLogParser.Reports
{
  public class WarningByProjectSummaryReport // : Report<WarningByProjectSummaryReportArgs>
  {/*
    private IList<string> ReportRows;
    private IList<ProjectWarningDataRow> ReportDataRows;
    public WarningByProjectSummaryReport() : base()
    {
      ReportRows = new List<string>();
      ReportDataRows = new Dictionary<string, int>();
    }

    public override IList<string> GetReportRows()
    {
      BuildReportRows();
      return ReportRows;
    }

    public override string GenerateReportRow(WarningByProjectSummaryReportArgs args)
    {
      VerifyWarningInSummary(args.WarningName);
      IncrementWarningCountInSummary(args.WarningName);
      return string.Empty;
    }

    private IList<string> BuildReportRows()
    {
      var sortedKeyList = SortReportDataRows();
      foreach (var warningName in sortedKeyList)
      {
        var warningCount = ReportDataRows[warningName];
        var reportRow = $"Warning: {warningName}:{warningCount}";
        ReportRows.Add(reportRow);
      }

      return ReportRows;
    }

    private IList<string> SortReportDataRows()
    {
      var sortedKeyList = ReportDataRows.Keys.ToList();
      sortedKeyList.Sort();
      return sortedKeyList;
    }

    private void VerifyWarningInSummary(string warningName)
    {
      var warningIsNotPresent = !ReportDataRows.ContainsKey(warningName);
      if (warningIsNotPresent)
      {
        ReportDataRows.Add(warningName, 0);
      }
    }

    private void IncrementWarningCountInSummary(string warningName)
    {
      ReportDataRows[warningName]++;
    } */

  }
  /*
  public class WarningByProjectSummaryReportArgs : ReportArgs
  {
    public string ProjectName { get; set; }
    public string WarningName { get; set; }
  }*/
}
