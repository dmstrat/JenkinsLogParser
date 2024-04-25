using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JenkinsLogParser.Tokens
{
  public static class TokenRegistry
  {
    public static IList<IToken> Tokens => GetTokens();

    private static IList<IToken> _Tokens;

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
      _Tokens = GetInstances<IToken>();
    }

    private static List<T> GetInstances<T>()
    {
      return (from t in Assembly.GetExecutingAssembly().GetTypes()
              where t.GetInterfaces().Contains(typeof(T)) && t.GetConstructor(Type.EmptyTypes) != null
              select (T)Activator.CreateInstance(t)).ToList();
    }
  }
}
