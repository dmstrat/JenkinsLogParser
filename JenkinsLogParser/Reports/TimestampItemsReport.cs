using System;
using System.Collections.Generic;
using System.Linq;
using JenkinsLogParser.DataModels;

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

    public override string GetReportName()
    {
      return "Timestamp Items";
    }

    public override string GetReportRowHeaders()
    {
      return "[LineNumber]:[Line Text]:(from last)[Duration from previous timestamp]: | (to next) [Duration to next timestamp]";
    }

    public override IList<string> GetReportRows()
    {
      ReportRows = new List<string>();
      BuildPaddingNumbers();
      BuildReportRows();
      return ReportRows;
    }

    public override void AddDataRow(TimestampItemsReportArgs args)
    {
      var reportRow = GenerateDataRowFromArguments(args);
      ReportDataRows.Add(reportRow);
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
      var previousTimeSpan = reportDataRows[0].TimeSpan;
      var nextTimeSpan = reportDataRows[1].TimeSpan;
      var currentTimeSpan = reportDataRows[0].TimeSpan;
      var currentRowText = reportDataRows[0].LineText;
      var currentRowLineNumber = reportDataRows[0].LineNumber;
      var firstReportRow = BuildReportRow(currentRowLineNumber, currentRowText, currentTimeSpan, previousTimeSpan, nextTimeSpan);
      ReportRows.Add(firstReportRow);

      for (int i = 1; i < ReportDataRows.Count - 1; i++)
      {
        previousTimeSpan = reportDataRows[i - 1].TimeSpan;
        nextTimeSpan = reportDataRows[i + 1].TimeSpan;
        currentTimeSpan = reportDataRows[i].TimeSpan;
        currentRowText = reportDataRows[i].LineText;
        currentRowLineNumber = reportDataRows[i].LineNumber;
        var newReportRow = BuildReportRow(currentRowLineNumber, currentRowText, currentTimeSpan, previousTimeSpan, nextTimeSpan);
        ReportRows.Add(newReportRow);
      }

      previousTimeSpan = reportDataRows[^2].TimeSpan;
      nextTimeSpan = reportDataRows[^1].TimeSpan;
      currentTimeSpan = reportDataRows[^1].TimeSpan;
      currentRowText = reportDataRows[^1].LineText;
      currentRowLineNumber = reportDataRows[^1].LineNumber;
      var lastReportRow = BuildReportRow(currentRowLineNumber, currentRowText, currentTimeSpan, previousTimeSpan, nextTimeSpan);
      ReportRows.Add(lastReportRow);

    }

    private string BuildReportRow(in long currentRowLineNumber, string currentRowText, in TimeSpan currentTimeSpan, in TimeSpan previousTimeSpan, in TimeSpan nextTimeSpan)
    {
      string durationFormat = @"hh\:mm\:ss\.ffff";
      var durationUp = (currentTimeSpan - previousTimeSpan).ToString(durationFormat);
      var durationDown = (nextTimeSpan - currentTimeSpan).ToString(durationFormat);
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

    private TimestampDataRow GenerateDataRowFromArguments(TimestampItemsReportArgs args)
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
}
