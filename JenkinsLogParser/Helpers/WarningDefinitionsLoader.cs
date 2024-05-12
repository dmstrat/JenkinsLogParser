using JenkinsLogParser.DataModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace JenkinsLogParser.Helpers
{
  internal static class WarningDefinitionsLoader
  {
    private const string WARNINGS_DEFINITION_JSON = @"Data/WarningDefinitions.json";

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
      PropertyNameCaseInsensitive = true
    };

    public static Dictionary<string, string> LoadWarnings()
    {
     var warningsDefinitions = JsonFileReader.Read<WarningDefinitionResource>(WARNINGS_DEFINITION_JSON);
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
        var text = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(text, JsonSerializerOptions);
      }
    }
  }
}