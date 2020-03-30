using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JenkinsLogParser
{
  internal class WarningDictionary : Dictionary<string, Dictionary<string, int>>
  {
    private readonly Dictionary<string, Dictionary<string, int>> _Dictionary;

    public WarningDictionary()
    {
      _Dictionary = new Dictionary<string, Dictionary<string, int>>();
    }

    public IDictionary<string, int> AddProject(string projectName)
    {
      AddProjectIfMissing(projectName);
      var returnValue = _Dictionary[projectName];
      return returnValue;
    }

    public KeyValuePair<string, int> AddWarning(string projectName, string warningName)
    {
      AddProjectIfMissing(projectName);
      AddWarningIfMissing(projectName, warningName);
      IncrementWarningForProject(projectName, warningName);
      var returnValue = new KeyValuePair<string, int>();
      return returnValue;
    }

    private KeyValuePair<string, Dictionary<string, int>> AddProjectIfMissing(string projectName)
    {
      var noProject = !_Dictionary.ContainsKey(projectName);
      if (noProject)
      {
        var newWarnings = new Dictionary<string, int>();
        _Dictionary.Add(projectName, newWarnings);
      }
      var newKvp = new KeyValuePair<string, Dictionary<string, int>>(projectName, _Dictionary[projectName]);
      return newKvp;
    }

    private void AddWarningIfMissing(string projectName, string warningName)
    {
      var noWarningInProject = !_Dictionary[projectName].ContainsKey(warningName);
      if (noWarningInProject)
      {
        _Dictionary[projectName].Add(warningName, 0);
      }
    }

    private KeyValuePair<string, int> IncrementWarningForProject(string projectName, string warningName)
    {
      _Dictionary[projectName][warningName]++;
      var newKvp = new KeyValuePair<string,int>(warningName, _Dictionary[projectName][warningName]);
      return newKvp;
    }

    public void IncrementWarning(string projectName, string warningName)
    {
      var warningExistsInProject = _Dictionary[projectName].ContainsKey(warningName);
      if (warningExistsInProject)
      {
        _Dictionary[projectName][warningName]++;
      }
      else
      {
        _Dictionary[projectName].Add(warningName, 1);
      }

    }
  }
}
