using JenkinsLogParser.Events.Projects;
using System.Collections.Generic;

namespace JenkinsLogParser.Handlers
{
  public class WarningHandler : IHandles<ProjectStarted>,
                                IHandles<WarningAdded>,
                                IHandles<ProjectEnded>
  {
    public const string EXTERNAL = "EXTERNAL";
    public static Dictionary<string, Dictionary<string, int>> ProjectWarningCount;
    public static string CurrentProject => _ProjectStack.Peek();
    private static Stack<string> _ProjectStack;

    public WarningHandler()
    {
      _ProjectStack = new Stack<string>();
      ProjectWarningCount = new Dictionary<string, Dictionary<string, int>>();
      AddOutsideProjectWarningsGroup();
    }

    public void Handle(ProjectStarted tokenEvent)
    {
      AddProjectToStack(tokenEvent.ProjectName);
      VerifyCurrentProjectExists(CurrentProject);
    }

    public void Handle(ProjectEnded tokenEvent)
    {
      PopFromStack();
    }

    private void PopFromStack()
    {
      _ProjectStack.Pop();
    }

    public void Handle(WarningAdded tokenEvent)
    {
      VerifyCurrentWarningExistsInCurrentProject(tokenEvent.WarningName);
    }

    private void VerifyCurrentWarningExistsInCurrentProject(string warningName)
    {
      if (warningName.Trim().Length < 1)
      {
        return;
      }
      VerifyCurrentProjectExists(CurrentProject);
      var warningExistsInProject = ProjectWarningCount[CurrentProject].ContainsKey(warningName);
      if (warningExistsInProject)
      {
        ProjectWarningCount[CurrentProject][warningName]++;
      }
      else
      {
        AddWarningToProject(warningName);
      }
    }

    private void AddWarningToProject(string warningName)
    {
      ProjectWarningCount[CurrentProject].Add(warningName, 1);
    }

    private void AddOutsideProjectWarningsGroup()
    {
      var newEntry = new Dictionary<string, int>();
      ProjectWarningCount.Add(EXTERNAL, newEntry);
      AddProjectToStack(EXTERNAL);
    }

    private void AddProjectToStack(string projectName)
    {
      _ProjectStack.Push(projectName);
    }

    private void VerifyCurrentProjectExists(string projectName)
    {
      if (!ProjectWarningCount.ContainsKey(projectName))
      {
        var newEntry = new Dictionary<string, int>();
        ProjectWarningCount.Add(projectName, newEntry);
      }
    }
  }
}
