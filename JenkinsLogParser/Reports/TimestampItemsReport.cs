using System;
using System.Collections.Generic;
using System.Linq;

namespace JenkinsLogParser.Reports
{
  public class TimestampItemsReport : Report<TimestampItemsReportArgs>
  {
    private int _LineNumberWidth = 6;
    private int _TextPaddingWidth = 50;
    private IList<TimestampDataRow> ReportDataRows;
    private IList<string> ReportRows;
    public TimestampItemsReport() : base()
    {
      ReportDataRows = new List<TimestampDataRow>();
    }

    public override IList<string> GetReportRows()
    {
      ReportRows = new List<string>();
      BuildPaddingNumbers();
      BuildReportRows();
      return ReportRows;
    }

    private void BuildPaddingNumbers()
    {
      var maxNumber = ReportDataRows.Max(row => row.LineNumber);
      var maxNumberStringLength = maxNumber.ToString().Length;
      _LineNumberWidth = maxNumberStringLength;

      var longestStringLength = ReportDataRows.Max(row => row.LineText.Length);
      _TextPaddingWidth = longestStringLength;
    }

    private void BuildReportRows()
    {
      switch (ReportDataRows.Count)
      {
        case 0:
          return;
        case 1:
          BuildSingleRowWithoutDurations(ReportDataRows.First());
          return;
        case 2:
          BuildTwoRowReport(ReportDataRows);
          return;
        default:
          BuildMultiLineReport(ReportDataRows);
          break;
      }
    }

    private void BuildMultiLineReport(IList<TimestampDataRow> reportDataRows)
    {
      TimeSpan previousTimeSpan;
      TimeSpan nextTimeSpan;
      TimeSpan currentTimeSpan;
      string currentRowText;
      long currentRowLineNumber;
      for (int i = 1; i < ReportDataRows.Count - 1; i++)
      {
        previousTimeSpan = ReportDataRows[i - 1].TimeSpan;
        nextTimeSpan = ReportDataRows[i + 1].TimeSpan;
        currentTimeSpan = ReportDataRows[i].TimeSpan;
        currentRowText = ReportDataRows[i].LineText;
        currentRowLineNumber = ReportDataRows[i].LineNumber;
        var newReportRow = BuildReportRow(currentRowLineNumber, currentRowText, currentTimeSpan, previousTimeSpan, nextTimeSpan);
        ReportRows.Add(newReportRow);
      }
    }

    private string BuildReportRow(in long currentRowLineNumber, string currentRowText, in TimeSpan currentTimeSpan, in TimeSpan previousTimeSpan, in TimeSpan nextTimeSpan)
    {
      var durationUp = currentTimeSpan - previousTimeSpan;
      var durationDown = nextTimeSpan - currentTimeSpan;
      var lineNumberRightAligned = currentRowLineNumber.ToString().PadLeft(_LineNumberWidth);
      var textLeftAligned = currentRowText.PadRight(_TextPaddingWidth);
      var newReportRowText = $"{lineNumberRightAligned}:{textLeftAligned} => (from last):{durationUp} | (to next):{durationDown}";
      return newReportRowText;
    }

    private void BuildSingleRowWithoutDurations(TimestampDataRow row)
    {
      var lineNumberRightAligned = row.LineNumber.ToString().PadLeft(_LineNumberWidth);
      var reportRow = $"{lineNumberRightAligned}:{row.LineText}";
      ReportRows.Add(reportRow);
    }

    private void BuildTwoRowReport(IList<TimestampDataRow> rows)
    {
      var lineNumberRightAligned = rows[0].LineNumber.ToString().PadLeft(_LineNumberWidth);
      var durationDown = rows[1].TimeSpan - rows[0].TimeSpan;
      var reportRow = $"{lineNumberRightAligned}:{rows[0].LineText}";
      ReportRows.Add(reportRow);

      lineNumberRightAligned = rows[1].LineNumber.ToString().PadLeft(_LineNumberWidth);
      reportRow = $"{lineNumberRightAligned}:{rows[1].LineText}";
      ReportRows.Add(reportRow);
    }

    public override string GenerateReportRow(TimestampItemsReportArgs args)
    {
      var reportRow = GenerateReportRowFromArguments(args);
      ReportDataRows.Add(reportRow);
      return string.Empty;
    }

    private TimestampDataRow GenerateReportRowFromArguments(TimestampItemsReportArgs args)
    {
      var timespanDataRow = new TimestampDataRow
      {
        LineNumber = args.LineNumber,
        LineText = args.LineText.Trim(),
        TimeSpan = args.Timespan
      };
      return timespanDataRow;
    }


  }

  public class TimestampItemsReportArgs : ReportArgs
  {
    public long LineNumber { get; set; }
    public string LineText { get; set; }
    public TimeSpan Timespan { get; set; }
  }

  public class TimestampDataRow
  {
    public long LineNumber { get; set; }
    public string LineText { get; set; }
    public TimeSpan TimeSpan { get; set; }
  }
}
