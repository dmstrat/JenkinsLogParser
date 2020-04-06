using System;

namespace JenkinsLogParser.DataModels
{
  public class TimestampDataRow
  {
    public long LineNumber { get; set; }
    public string LineText { get; set; }
    public TimeSpan TimeSpan { get; set; }
  }
}