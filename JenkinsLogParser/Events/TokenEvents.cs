using System;
using System.Collections.Generic;

namespace JenkinsLogParser.Events
{
  public static class TokenEvents
  {
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

    private static IList<Delegate> _Actions;

    public static IList<Delegate> Actions
    {
      get
      {
        var actions = _Actions;
        if (actions == null)
        {
          actions = new List<Delegate>();
          Actions = actions;
          RegisterDefaultActions();
        }
        return actions;
      }
      set => _Actions = value;
    }

    private static void Register<T>(Action<T> callback) where T : ITokenEvent
    {
      Actions.Add(callback);
    }

    private static void RegisterDefaultActions()
    {
      RegisterDefaultHandlers();
    }

    private static void RegisterDefaultHandlers()
    {
      //var projectHandler = new ProjectHandler();
      //Register<ProjectStarted>(projectHandler.Handle);
      //Register<WarningAdded>(projectHandler.Handle);
      //Register<ProjectEnded>(projectHandler.Handle);
      //var warningHandler = new WarningHandler();
      //Register<ProjectStarted>(warningHandler.Handle);
      //Register<WarningAdded>(warningHandler.Handle);
      //Register<ProjectEnded>(warningHandler.Handle);
    }
  }
}
