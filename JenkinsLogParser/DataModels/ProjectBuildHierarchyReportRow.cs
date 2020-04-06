using JenkinsLogParser.Reports;

namespace JenkinsLogParser.DataModels
{
  public class ProjectBuildHierarchyReportRow
  {
    public long LineNumber { get; set; }
    public string ProjectName { get; set; }
    public ProjectAction Action { get; set; }
  }
}