using System;
using System.Text.RegularExpressions;

namespace JenkinsLogParser.Helpers
{
  public static class TimeHelper
  {
    public static TimeSpan GenerateTimestampFromLine(string line)
    {
      var returnValue = new TimeSpan();
      var timestampRegEx = new Regex(@"\d+\:\d+\:\d+\.\d{0,3}");
      var result = timestampRegEx.Match(line);
      if (result.Success)
      {
        var split = result.Value.Split(':');
        if (split.Length > 2)
        {
          var split2 = split[2].Split('.');
          var hours = Convert.ToInt32(split[0]);
          var minutes = Convert.ToInt32(split[1]);
          if (split2.Length > 1)
          {   
            var seconds = Convert.ToInt32(split2[0]);
            var milliseconds = TimeHelper.ConvertToMilliseconds(split2[1]);
            returnValue = new TimeSpan(0, hours, minutes, seconds, milliseconds);
          }
          else
          {
            var seconds = Convert.ToInt32(split[2]);
            returnValue = new TimeSpan(0, hours, minutes, seconds);
          }
        }
      }
      else
      {
        returnValue = TimeSpan.Zero;
      }

      return returnValue;
    }

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
