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
      var token = new ProjectBuildEndline();
      var tokenMatchesLine = token.IsMatchForThisToken(testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var expectedLine = "Project:EsoSmApi END ";
      var actualLine = token.GetLine();
      Assert.AreEqual(expectedLine, actualLine);
    }
  }
}
