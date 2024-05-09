using System.Collections;
using System.Collections.Generic;

namespace JenkinsLogParser.Reports
{
  public abstract class Report<T> : Report where T : ReportArgs
  {
    /// <summary>
    /// Gets the name of the report
    /// </summary>
    /// <returns>name of report.  Empty string if not overridden by inheriting report.</returns>
    public abstract string GetReportName();

    /// <summary>
    /// Gets the header names for this report.
    /// Ex. [Line Number]:[Action]:[SupportText]
    /// </summary>
    /// <returns>empty string if not overridden by inheriting report.</returns>
    public abstract string GetReportRowHeaders();

    /// <summary>
    /// Adds a data row to the report to be used as the data source for the report.
    /// </summary>
    /// <param name="args">The class used as the source for the data row.</param>
    public abstract void AddDataRow(T args);
  }

  public class Report : IEnumerable<string> 
  {
    private IList<string> _Rows = new List<string>();
    public IList<string> Rows => _Rows;

    /// <summary>
    /// Adds a line to the report
    /// </summary>
    /// <param name="lineToAdd"></param>
    public virtual void AddRow(string lineToAdd)
    {
      _Rows.Add(lineToAdd);
    }

    /// <summary>
    /// Gets the Report Data
    /// </summary>
    /// <returns>the report's data as a list.</returns>
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
