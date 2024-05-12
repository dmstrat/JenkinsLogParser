using System.Collections.Generic;
using JenkinsLogParser.Events.DotCover;
using JenkinsLogParser.Reports;

namespace JenkinsLogParser.Handlers
{
    public class DotCoverHandler : IHandles<DotCoverStarted>, IHandles<DotCoverEnded>, IHandles<DotCoverCoverExecutionAdded>
  {
    private DotCoverItemsReport _DotCoverItemsReport;

    public DotCoverHandler(ref DotCoverItemsReport dotCoverItemsReport)
    {
      _DotCoverItemsReport = dotCoverItemsReport;
    }

    public void Handle(DotCoverCoverExecutionAdded tokenEvent)
    {
      _DotCoverItemsReport.AddDataRow(tokenEvent.LineNumber, tokenEvent.FullText, DotCoverAction.Execute, tokenEvent.TestCategory);
    }

    public void Handle(DotCoverStarted tokenEvent)
    {
      var report = _DotCoverItemsReport;
      report.AddDataRow(tokenEvent.LineNumber, tokenEvent.FullText, DotCoverAction.Start, tokenEvent.Timestamp);
    }

    public void Handle(DotCoverEnded tokenEvent)
    {
      _DotCoverItemsReport.AddDataRow(tokenEvent.LineNumber, tokenEvent.FullText, DotCoverAction.End, tokenEvent.Timestamp);
    }
  }
}
