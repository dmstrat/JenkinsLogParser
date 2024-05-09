using System.Collections.Generic;
using System.Linq;

namespace JenkinsLogParser.Reports
{
  public class WarningSummaryReport : Report<WarningSummaryReportArgs>
  {
    private IList<string> _ReportRows;
    private IDictionary<string, int> _ReportDataRows;
    private int _CountPadding;
    private int _WarningPadding;
    public WarningSummaryReport() : base()
    {
      _ReportRows = new List<string>();
      _ReportDataRows = new Dictionary<string, int>();
    }

    public override string GetReportName()
    {
      return "Warning Summary";
    }

    public override string GetReportRowHeaders()
    {
      return "Warning: [WarningName]:[Count]";
    }

    public override IList<string> GetReportRows()
    {
      BuildPaddingNumbers();
      BuildReportRows();
      return _ReportRows;
    }

    private void BuildPaddingNumbers()
    {
      var maxWarningNameLength = _ReportDataRows.Max(row => row.Key.Length);
      _WarningPadding = maxWarningNameLength;

      var maxCountLength = _ReportDataRows.Max(row => row.Value.ToString().Length);
      _CountPadding = maxCountLength;
    }

    public override void AddDataRow(WarningSummaryReportArgs args)
    {
      VerifyWarningInSummary(args.WarningName);
      IncrementWarningCountInSummary(args.WarningName);
    }

    private void BuildReportRows()
    {
      var sortedKeyList = SortReportDataRows();
      foreach (var warningName in sortedKeyList)
      {
        var warningNameLeftAligned = warningName.PadRight(_WarningPadding);
        var warningCountRightAligned = _ReportDataRows[warningName].ToString().PadLeft(_CountPadding);
        var reportRow = $"Warning: {warningNameLeftAligned}:{warningCountRightAligned }";
        _ReportRows.Add(reportRow);
      }
    }

    private IList<string> SortReportDataRows()
    {
      var sortedKeyList = _ReportDataRows.Keys.ToList();
      sortedKeyList.Sort();
      return sortedKeyList;
    }

    private void VerifyWarningInSummary(string warningName)
    {
      var warningIsNotPresent = !_ReportDataRows.ContainsKey(warningName);
      if (warningIsNotPresent)
      {
        _ReportDataRows.Add(warningName, 0);
      }
    }

    private void IncrementWarningCountInSummary(string warningName)
    {
      _ReportDataRows[warningName]++;
    }
  }

  public class WarningSummaryReportArgs : ReportArgs
  {
    public string WarningName { get; set; }
  }
}
