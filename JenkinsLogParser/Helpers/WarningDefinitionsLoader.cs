using JenkinsLogParser.DataModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace JenkinsLogParser.Helpers
{
  internal static class WarningDefinitionsLoader
  {
    public static Dictionary<string, string> LoadWarnings()
    {
     var warningsDefinitions = JsonFileReader.Read<WarningDefinitionSource>(@"Data/WarningDefinitions.json");
     var returnCollection = new Dictionary<string, string>();
     foreach (var definition in warningsDefinitions.Definitions)
     {
       var tryingToLoadDuplicate = returnCollection.ContainsKey(definition.Name);
       if (tryingToLoadDuplicate) 
       {
         Trace.WriteLine($"duplicate entry for warning {definition.Name}!  Remove duplicate from source file.");
         continue;
       }
       returnCollection.Add(definition.Name, definition.Description);
     }

     return returnCollection;
    }

    private static class JsonFileReader
    {
      public static T Read<T>(string filePath)
      {
        var options = new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        };
        string text = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(text, options);
      }
    }
  }
}