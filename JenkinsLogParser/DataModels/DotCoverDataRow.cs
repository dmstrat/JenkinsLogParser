using System;
using JenkinsLogParser.Reports;

namespace JenkinsLogParser.DataModels
{
  public class DotCoverDataRow
  {
    public long LineNumber { get; set; }
    public string LineText { get; set; }
    public DotCoverAction Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string TestCategory { get; set; }
  }
}