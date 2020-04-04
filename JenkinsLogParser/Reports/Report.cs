using System.Collections;
using System.Collections.Generic;

namespace JenkinsLogParser.Reports
{
  public abstract class Report<T> : Report where T : ReportArgs 
  {
    public abstract string GenerateReportRow(T args);
  }

  public class Report : IEnumerable<string> 
  {
    private IList<string> _Rows = new List<string>();
    public IList<string> Rows => _Rows;
    public virtual void AddRow(string lineToAdd)
    {
      _Rows.Add(lineToAdd);
    }

    public virtual IList<string> GetReportRows()
    {
      return _Rows;
    }
    public IEnumerator<string> GetEnumerator()
    {
      return _Rows.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
