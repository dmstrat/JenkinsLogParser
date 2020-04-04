using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JenkinsLogParser.Reports
{
  public static class ReportRegistry//<U> 
    //where U : ReportArgs
  {
    //public static IList<Report> Reports => GetReports();
    //public static IList<Report<U>> Reports => GetReports();
    public static IList<Report<ReportArgs>> Reports => GetReports();

    //private static IList<Report> _Reports { get; set; }
    //private static IList<Report<U>> _Reports { get; set; }
    private static IList<Report<ReportArgs>> _Reports { get; set; }

    //private static IList<Report> GetReports()
    //private static IList<Report<U>> GetReports()
    private static IList<Report<ReportArgs>> GetReports()
    {
      if (_Reports == null)
      {
        BuildReports();
      }
      return _Reports;
    }

    //public static Report<U> GetReport(Type reportType)
    public static Report<ReportArgs> GetReport(Type reportType)
    {
      var foundReport = GetReports().FirstOrDefault(r => r.GetType() == reportType);
      return foundReport;
    }

    private static void BuildReports()
    {
      //_Reports = GetInstances<Report>();
      //var reports = GetInstances<Report<U>>();
      var reports = GetInstances<Report<ReportArgs>>();
      _Reports = reports;// GetInstances<Report<ReportArgs>>();
    }

    private static IList<T> GetInstances<T>()
    {
      return (from t in Assembly.GetExecutingAssembly().GetTypes()
              where t.BaseType == (typeof(T)) && t.GetConstructor(Type.EmptyTypes) != null
              select (T)Activator.CreateInstance(t)).ToList();
    }

    /*private static List<T> GetInstances<T>()
    {
      return (from t in Assembly.GetExecutingAssembly().GetTypes()
              where t.GetInterfaces().Contains(typeof(T)) && t.GetConstructor(Type.EmptyTypes) != null
              select (T)Activator.CreateInstance(t)).ToList();
    }*/

    private static IList<Report<ReportArgs>> BuildReportList()
    {
      var returnList = new List<Report<ReportArgs>>();
      var projectBuildReport = new ProjectBuildHierarchyReport();
      //returnList.Add(projectBuildReport);

      return returnList;

    }

  }
}

