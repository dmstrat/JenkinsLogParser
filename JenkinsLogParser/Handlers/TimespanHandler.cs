using JenkinsLogParser.Events;
using JenkinsLogParser.Reports;

namespace JenkinsLogParser.Handlers
{
  public class TimespanHandler : IHandles<TimestampAdded>
  {
    private readonly TimestampItemsReport _TimespanItemsReport;

    public TimespanHandler(ref TimestampItemsReport timestampItemsReport)
    {
      _TimespanItemsReport = timestampItemsReport;
    }

    public void Handle(TimestampAdded tokenEvent)
    {
      var reportArgs = new TimestampItemsReportArgs
      {
        LineNumber = tokenEvent.LineNumber,
        LineText = tokenEvent.FullText,
        Timespan = tokenEvent.TimeSpan
      };
      _TimespanItemsReport.AddDataRow(reportArgs);
    }
  }
}
