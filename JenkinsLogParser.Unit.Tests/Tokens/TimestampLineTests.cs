using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JenkinsLogParser.Tokens;

namespace JenkinsLogParser.Unit.Tests.Tokens
{
  [TestClass]
  public class TimestampLineTests
  {
    [TestMethod]
    public void WhenLineIsTrimmedOnStartAndEndWeCanMatchThreeDollarSignsAtStart()
    {
      var testLine = "  $$$ Start Reporting Platform:  7:45:42.44  ";
      var token = TimestampLine.ProcessLine(testLine);
      Assert.IsNotNull(token);
      var actualTimespan = token.GetTimespan();
      var expectedTimespan = new TimeSpan(0,7, 45, 42, 440);
      Assert.AreEqual(expectedTimespan, actualTimespan);
    }
  }
}
