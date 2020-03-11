using System;

namespace JenkinsLogParser.Helpers
{
  public static class TimeHelper
  {
    public static int ConvertToMilliseconds(string millisecondsAsString)
    {
      var stringLength = millisecondsAsString.Length;
      switch (stringLength)
      {
        case 0:
          return 0;
        case 1:
          return Convert.ToInt32(millisecondsAsString) * 100;
        case 2:
          return Convert.ToInt32(millisecondsAsString) * 10;
        case 3:
          return Convert.ToInt32(millisecondsAsString);
        default:
          return Convert.ToInt32(millisecondsAsString.Substring(0, 3));
      }
    }
  }
}
