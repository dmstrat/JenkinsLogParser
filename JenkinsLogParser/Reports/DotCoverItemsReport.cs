using JenkinsLogParser.DataModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JenkinsLogParser.Reports
{
  public class DotCoverItemsReport : Report<DotCoverItemsReportArgs>
  {
    private TimeSpan MinTimeSpan = TimeSpan.MaxValue;
    private TimeSpan MaxTimeSpan = TimeSpan.MinValue;
    private int _LineNumberWidth = 6;
    private int _TextPaddingWidth = 50;
    private IList<DotCoverDataRow> ReportDataRows = new List<DotCoverDataRow>();
    private IList<string> ReportRows;

    public override string GetReportName()
    {
      return "DotCover Items";
    }

    public override string GetReportRowHeaders()
    {
      return "[Execute Line Number | Start - End Line Numbers]:[Category]:[Duration]";
    }

    public override IList<string> GetReportRows()
    {
      ReportRows = new List<string>();
      BuildPaddingNumbers();
      BuildReportRows();
      return ReportRows;
    }

    internal void AddDataRow(long lineNumber, string fullText, DotCoverAction action, string testCategory)
    {
      var dotCoverDataRow = new DotCoverDataRow
      {
        LineNumber = lineNumber,
        LineText = fullText.Trim(),
        Action = action,
        TestCategory = testCategory
      };
      ReportDataRows.Add(dotCoverDataRow);
    }

    internal void AddDataRow(long lineNumber, string fullText, DotCoverAction action, DateTime timestamp)
    {
      var dotCoverDataRow = new DotCoverDataRow
      {
        LineNumber = lineNumber,
        LineText = fullText.Trim(),
        Timestamp = timestamp,
        Action = action
      };
      ReportDataRows.Add(dotCoverDataRow);
    }

    public override void AddDataRow(DotCoverItemsReportArgs args)
    {
    }

    private void BuildPaddingNumbers()
    {
      var maxNumber = ReportDataRows.Max(row => row.LineNumber);
      var maxNumberStringLength = maxNumber.ToString().Length;
      _LineNumberWidth = maxNumberStringLength;

      var executeDataRows = ReportDataRows.Where(x => x.Action == DotCoverAction.Execute);

      var longestTestCategoryLength = executeDataRows.Max(row => row.TestCategory.Length);
      _TextPaddingWidth = longestTestCategoryLength;
    }

    private void BuildReportRows()
    {
      BuildMultiLineReport(ReportDataRows);
    }

    private void BuildMultiLineReport(IList<DotCoverDataRow> reportDataRows)
    {
      //Sort by LineNumber 
      var sortedRows = reportDataRows.OrderBy(x => x.LineNumber).ToList();

      var weDoNotHaveStartEndPairsForEveryStartEnd = sortedRows.Count() % 3 != 0;
      if (weDoNotHaveStartEndPairsForEveryStartEnd)
      {
        Trace.WriteLine("missing a triple of dotcover commands - might have failed mid-execution.");
        //throw new NotImplementedException("Report Has missing start/end pair items for DotCover Report");
      }

      for (int i = 0; i < sortedRows.Count() - 2;)//; i = i + 2)
      {
        var thereAreThreeRecordsLeftToTest = i + 2 < sortedRows.Count();
        if (thereAreThreeRecordsLeftToTest)
        {
          var haveValidTriplet = sortedRows[i].Action == DotCoverAction.Execute &&
                                    sortedRows[i + 1].Action == DotCoverAction.Start &&
                                    sortedRows[i + 2].Action == DotCoverAction.End;
          if (haveValidTriplet)
          {
            //perform triplet action 
            var execution = sortedRows[i];
            var start = sortedRows[i + 1];
            var end = sortedRows[i + 2];
            var reportRow = BuildReportRow(execution, start, end);
            ReportRows.Add(reportRow);
            i += 2; //move to next pair 
          }
          else
          {
            Trace.WriteLine("skipping record for failed triplet");
            i++; //move forward one to try and get back in line for pairs
            //TODO: should we report the lines where this problem is? How would that look?
          }
        }
        else
        {
          Trace.WriteLine("invalid triplet");
          //TODO:what do we do with this orphan last record? Should it match the 'error' record but only one row to report?
        }
      }

      var footerRow = BuildFooter();
      ReportRows.Add(footerRow);
    }

    private string BuildFooter()
    {
      var minReport = $"Min Runtime:{MinTimeSpan.ToString()}";
      var maxReport = $"Max Runtime:{MaxTimeSpan.ToString()}";
      var returnValue = $"{minReport} - {maxReport}";
      return returnValue;
    }

    private string BuildReportRow(in DotCoverDataRow executionRow, in DotCoverDataRow startRow, in DotCoverDataRow endRow)
    {
      var timeSpan = endRow.Timestamp - startRow.Timestamp;
      var longerThanMax = timeSpan > MaxTimeSpan;
      if (longerThanMax)
      {
        MaxTimeSpan = timeSpan;
      }
      var shorterThanMin = timeSpan < MinTimeSpan;
      if (shorterThanMin)
      {
        MinTimeSpan = timeSpan;
      }

      var executionLineNumber = executionRow.LineNumber.ToString().PadLeft(_LineNumberWidth);
      var startLineNumber = startRow.LineNumber.ToString().PadLeft(_LineNumberWidth);
      var endLineNumber = endRow.LineNumber.ToString().PadLeft(_LineNumberWidth);
      var testCategory = executionRow.TestCategory.Trim().PadLeft(_TextPaddingWidth);
      var diff = (endRow.Timestamp - startRow.Timestamp).ToString();

      var reportLine = $"{executionLineNumber}:{startLineNumber} - {endLineNumber}:{testCategory}:{diff}";

      return reportLine;
    }
  }

  public class DotCoverItemsReportArgs : ReportArgs
  {

  }

  public enum DotCoverAction
  {
    Execute,
    Start,
    End
  }
}
