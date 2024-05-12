using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsLogParser.DataModels
{
  internal class WarningDefinitionSource
  {
    public IList<WarningDefinition> Definitions { get; set; }
  }

  internal class WarningDefinition
  {
    public string Name { get; set; }
    public string Description { get; set; }
  }
}
