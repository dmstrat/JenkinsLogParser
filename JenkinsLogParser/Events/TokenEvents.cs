using JenkinsLogParser.Events.Projects;
using JenkinsLogParser.Handlers;
using System;
using System.Collections.Generic;

namespace JenkinsLogParser.Events
{
  public static class TokenEvents
  {
    private static List<Delegate> _Actions;

    private static List<Delegate> Actions
    {
      get
      {
        var actions = _Actions;
        if (actions == null)
        {
          actions = new List<Delegate>();
          Actions = actions;
          RegisterActions();
        }

        return actions;
      }
      set => _Actions = value;
    }

    private static void RegisterActions()
    {
      RegisterHandlers();
    }

    private static void RegisterHandlers()
    {
      var projectHandler = new ProjectHandler();
      Register<ProjectStarted>(projectHandler.Handle);
      var warningHandler = new WarningHandler();
      Register<ProjectStarted>(warningHandler.Handle);
      Register<WarningAdded>(warningHandler.Handle);
      Register<ProjectEnded>(warningHandler.Handle);
    }

    private static void Register<T>(Action<T> callback) where T : ITokenEvent
    {
      Actions.Add(callback);
    }

    public static void Raise<T>(T args) where T : ITokenEvent
    {
      var actions = Actions;
      foreach (var action in actions)
      {
        if (action is Action<T> actionObj)
        {
          ((Action<T>)action)(args);
        }
      }
    }
  }
}
