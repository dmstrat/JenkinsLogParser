using JenkinsLogParser.Tokens;
using JenkinsLogParser.Unit.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JenkinsLogParser.Unit.Tests.Tokens
{
  [TestClass]
  public class ProjectBuildEndLineTests
  {
    [TestMethod]
    public void WhenProjectEndLineIsProvidedWeFindAMatchAndGetEsoSmApiAsResult()
    {
      var lineNumber = 15;
      var testLine = "Done Building Project \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\BOS\\EsoSmApi\\EsoSmApi.csproj\" (default targets).";
      var token = new ProjectBuildEndLine();
      var tokenMatchesLine = token.ProcessLine(lineNumber, testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var expectedRegexMatch = "EsoSmApi";
      var actualEventExists = EventHelper.HasSpecificProjectEndedEvent(lineNumber, testLine, expectedRegexMatch);
      Assert.IsTrue(actualEventExists);

    }

    [TestMethod]
    public void WhenWixProjectEndLineIsProvidedWeFindAMatchAndGetnApeLibraryAsResult()
    {
      var lineNumber = 16;
      var testLine = "Done Building Project \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\Engines\\Deploy\\nApeLibrary\\nApeLibrary.wixproj\" (default targets).";
      var token = new ProjectBuildEndLine();
      var tokenMatchesLine = token.ProcessLine(lineNumber, testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var expectedRegexMatch = "nApeLibrary";
      var actualEventExists = EventHelper.HasSpecificProjectEndedEvent(lineNumber, testLine, expectedRegexMatch);
      Assert.IsTrue(actualEventExists);
    }
  }
}
