using System.Collections.Generic;

namespace JenkinsLogParser.DataModels
{
  internal class WarningDefinitionResource
  {
    public IEnumerable<WarningDefinition> Definitions { get; set; }
  }

  internal class WarningDefinition
  {
    public string Name { get; set; }
    public string Description { get; set; }
  }
}
