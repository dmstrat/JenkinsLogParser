using JenkinsLogParser.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace JenkinsLogParser.Unit.Tests.Tokens
{
  [TestClass]
  public class TimestampLineTests
  {
    [TestMethod]
    public void WhenLineIsTrimmedOnStartAndEndWeCanMatchThreeDollarSignsAtStart()
    {
      var testLine = "  $$$ Start Reporting Platform:  7:45:42.44  ";
      var token = new TimestampLine();
      var tokenMatchesLine = token.ProcessLine(0, testLine);// TimestampLine.ProcessLine(testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var actualTimespan = token.GetTimespan();
      var expectedTimespan = new TimeSpan(0,7, 45, 42, 440);
      Assert.AreEqual(expectedTimespan, actualTimespan);
    }
  }
}
