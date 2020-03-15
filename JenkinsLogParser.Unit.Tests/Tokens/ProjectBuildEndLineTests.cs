using JenkinsLogParser.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JenkinsLogParser.Unit.Tests.Tokens
{
  [TestClass]
  public class ProjectBuildEndLineTests
  {
    [TestMethod]
    public void WhenProjectEndLineIsProvidedWeFindAMatchAndGetEsoSmApiAsResult()
    {
      var testLine = "Done Building Project \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\BOS\\EsoSmApi\\EsoSmApi.csproj\" (default targets).";
      var token = new ProjectBuildEndLine();
      var tokenMatchesLine = token.IsMatchForThisToken(testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var expectedLine = "Project:EsoSmApi END ";
      var actualLine = token.GetLine();
      Assert.AreEqual(expectedLine, actualLine);
      var expectedMatch = "EsoSmApi";
      var actualMatch = token.GetMatch();
      Assert.AreEqual(expectedMatch, actualMatch);
    }

    [TestMethod]
    public void WhenWixProjectEndLineIsProvidedWeFindAMatchAndGetnApeLibraryAsResult()
    {
      var testLine = "Done Building Project \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\Engines\\Deploy\\nApeLibrary\\nApeLibrary.wixproj\" (default targets).";
      var token = new ProjectBuildEndLine();
      var tokenMatchesLine = token.IsMatchForThisToken(testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var expectedLine = "Project:nApeLibrary END ";
      var actualLine = token.GetLine();
      Assert.AreEqual(expectedLine, actualLine);
      var expectedMatch = "nApeLibrary";
      var actualMatch = token.GetMatch();
      Assert.AreEqual(expectedMatch, actualMatch);
    }
  }
}
