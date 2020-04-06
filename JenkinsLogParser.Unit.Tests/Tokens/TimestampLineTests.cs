using JenkinsLogParser.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using JenkinsLogParser.Unit.Tests.Helpers;

namespace JenkinsLogParser.Unit.Tests.Tokens
{
  [TestClass]
  public class TimestampLineTests
  {
    [ClassInitialize]
    public static void ClassInitialize(TestContext testContext)
    {
      var testDelegateHelper = new TestDelegateHelper();
      var testDelegateHandlers = testDelegateHelper.BuildTestDelegatesDictionary();
    }

    [TestMethod]
    public void WhenLineIsTrimmedOnStartAndEndWeCanMatchThreeDollarSignsAtStart()
    {
      var lineNumber = 18;
      var testLine = "  $$$ Start Reporting Platform:  7:45:42.44  ";
      var token = new TimestampLine();
      var tokenMatchesLine = token.ProcessLine(lineNumber, testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var actualTimespan = token.GetTimespan();
      var expectedTimespan = new TimeSpan(0,7, 45, 42, 440);
      Assert.AreEqual(expectedTimespan, actualTimespan);
      var expectedRegexMatch = testLine.Trim();
      var actualEventExists = EventHelper.HasSpecificTimestampAddedEvent(lineNumber, testLine, expectedRegexMatch);
      Assert.IsTrue(actualEventExists);
    }
  }
}
