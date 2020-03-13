using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JenkinsLogParser.Tokens;

namespace JenkinsLogParser.Unit.Tests.Tokens
{
  [TestClass]
  public class ProjectBuildStartLineTests
  {
    [TestMethod]
    public void WhenProjectStartLineIsProvidedWeFindAMatchAndGetPayEngineAsResult()
    {
      var testLine = "Project \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\Pay.sln\" (2) is building \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\Engines\\Pay\\PayEngine\\PayEngine.csproj\" (3) on node 1 (default targets).";
      var token = new ProjectBuildStartLine();
      var tokenMatchesLine = token.IsMatchForThisToken(testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var expectedLine = "Project:PayEngine START ";
      var actualLine = token.GetLine();
      Assert.AreEqual(expectedLine, actualLine);
    }

    [TestMethod]
    public void WhenProjectStartLineIsProvidedByProjectWeFindAMatchAndGetRPCoreAsResult()
    {
      var testLine = "Project \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\Common\\DomainModel\\DomainModel.csproj\" (4) is building \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\Common\\SharedBinaries\\Source\\RPCore\\RPCore.csproj\" (5:3) on node 1 (default targets).";
      var token = new ProjectBuildStartLine();
      var tokenMatchesLine = token.IsMatchForThisToken(testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var expectedLine = "Project:RPCore START ";
      var actualLine = token.GetLine();
      Assert.AreEqual(expectedLine, actualLine);
    }
  }
}
