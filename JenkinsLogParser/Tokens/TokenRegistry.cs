using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JenkinsLogParser.Tokens
{
  public static class TokenRegistry
  {
    public static IList<IToken> Tokens => GetTokens();// GetInstances<Token>();

    private static IList<IToken> _Tokens { get; set; }

    private static IList<IToken> GetTokens()
    {
      if (_Tokens == null)
      {
        BuildTokens();
      }
      return _Tokens;
    }

    private static void BuildTokens()
    {
      var tokenRegistry = new List<IToken>();

      tokenRegistry.Add(new TimestampLine());

      _Tokens = tokenRegistry;
    }

    private static IList<T> GetInstances<T>()
    {
      return (from t in Assembly.GetExecutingAssembly().GetTypes()
              where t.BaseType == (typeof(T)) && t.GetConstructor(Type.EmptyTypes) != null
              select (T)Activator.CreateInstance(t)).ToList();
    }

    //private static List<T> GetInstances<T>()
    //{
    //  return (from t in Assembly.GetExecutingAssembly().GetTypes()
    //          where t.GetInterfaces().Contains(typeof(T)) && t.GetConstructor(Type.EmptyTypes) != null
    //          select (T)Activator.CreateInstance(t)).ToList();
    //}
  }
}
