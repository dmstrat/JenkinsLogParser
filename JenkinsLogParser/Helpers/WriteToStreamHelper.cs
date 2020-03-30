using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JenkinsLogParser.Handlers;

namespace JenkinsLogParser.Helpers
{
  internal static class WriteToStreamHelper
  {
    public static void WriteWarningsToStream(StreamWriter streamWriter, WarningDictionary warningDictionary)
    {
      WriteLineToOutput(streamWriter, "Warnings for this BUILD (via Events)");
      var externalList = warningDictionary["EXTERNAL"];
      var projectList = warningDictionary.Where(x => x.Key != "EXTERNAL");
      var sortedProjectList = projectList.OrderBy(x => x.Key);
      foreach (var projectWarningList in sortedProjectList)
      {
        var worthPrinting = projectWarningList.Value.Count > 0;
        if (worthPrinting)
        {
          var currentProject = projectWarningList.Key;
          WriteLineToOutput(streamWriter, currentProject);
          var warningList = projectWarningList.Value;
          var sortedWarningList = from pair in warningList orderby pair.Key select pair;
          foreach (var warningCount in sortedWarningList)
          {
            var outputLine = "  " + warningCount.Key + ":" + warningCount.Value;
            WriteLineToOutput(streamWriter, outputLine);
          }
        }
      }

      WriteLineToOutput(streamWriter, "EXTERNAL TO PROJECTS");
      foreach (var warningItem in externalList)
      {
        var outputLine = "  " + warningItem.Key + ":" + warningItem.Value;
        WriteLineToOutput(streamWriter, outputLine);
      }
    }

    public static void WriteWarningsToStream(StreamWriter streamWriter, Dictionary<string, Dictionary<string, int>> warningDictionary)
    {
      WriteLineToOutput(streamWriter, "Warnings for this BUILD (via Events)");
      var externalList = warningDictionary["EXTERNAL"];
      var projectList = warningDictionary.Where(x => x.Key != "EXTERNAL");
      var sortedProjectList = projectList.OrderBy(x => x.Key);
      foreach (var projectWarningList in sortedProjectList)
      {
        var worthPrinting = projectWarningList.Value.Count > 0;
        if (worthPrinting)
        {
          var currentProject = projectWarningList.Key;
          WriteLineToOutput(streamWriter, currentProject);
          var warningList = projectWarningList.Value;
          var sortedWarningList = from pair in warningList orderby pair.Key select pair;
          foreach (var warningCount in sortedWarningList)
          {
            var outputLine = "  " + warningCount.Key + ":" + warningCount.Value;
            WriteLineToOutput(streamWriter, outputLine);
          }
        }
      }

      WriteLineToOutput(streamWriter, "EXTERNAL TO PROJECTS");
      foreach (var warningItem in externalList)
      {
        var outputLine = "  " + warningItem.Key + ":" + warningItem.Value;
        WriteLineToOutput(streamWriter, outputLine);
      }
    }

    public static void WriteOutputToStream(StreamWriter streamWriter, IList<string> output)
    {
      foreach (string line in output)
      {
        WriteLineToOutput(streamWriter, line);
      }
    }


    public static void WriteLineToOutput(StreamWriter streamWriter, string lineToRecord)
    {
      streamWriter.WriteLine(lineToRecord);
    }

  }
}
