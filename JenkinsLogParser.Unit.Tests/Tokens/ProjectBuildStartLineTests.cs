using JenkinsLogParser.Tokens;
using JenkinsLogParser.Unit.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JenkinsLogParser.Unit.Tests.Tokens
{
  [TestClass]
  public class ProjectBuildStartLineTests
  {
    [ClassInitialize]
    public static void ClassInitialize(TestContext testContext)
    {
      var testDelegateHelper = new TestDelegateHelper();
      var testDelegateHandlers = testDelegateHelper.BuildTestDelegatesDictionary();
    }

    [TestMethod]
    public void WhenProjectStartLineIsProvidedWeFindAMatchAndGetPayEngineAsResult()
    {
      var lineNumber = 12;
      var testLine = "Project \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\Pay.sln\" (2) is building \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\Engines\\Pay\\PayEngine\\PayEngine.csproj\" (3) on node 1 (default targets).";
      var token = new ProjectBuildStartLine();
      var tokenMatchesLine = token.ProcessLine(lineNumber, testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var expectedRegexMatch = "PayEngine";
      var actualEventExists = EventHelper.HasSpecificProjectStartEvent(lineNumber, testLine, expectedRegexMatch);
      Assert.IsTrue(actualEventExists);
    }

    [TestMethod]
    public void WhenProjectStartLineIsProvidedByProjectWeFindAMatchAndGetRPCoreAsResult()
    {
      var lineNumber = 13;
      var testLine = "Project \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\Common\\DomainModel\\DomainModel.csproj\" (4) is building \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\Common\\SharedBinaries\\Source\\RPCore\\RPCore.csproj\" (5:3) on node 1 (default targets).";
      var token = new ProjectBuildStartLine();
      var tokenMatchesLine = token.ProcessLine(lineNumber, testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var expectedRegexMatch = "RPCore";
      var actualEventExists = EventHelper.HasSpecificProjectStartEvent(lineNumber, testLine, expectedRegexMatch);
      Assert.IsTrue(actualEventExists);
    }

    [TestMethod]
    public void WhenWixProjectStartLineIsProvidedByProjectWeFindAMatchAndGetRPCoreAsResult()
    {
      var lineNumber = 14;
      var testLine = "Project \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\RetailInstaller.sln\" (173) is building \"C:\\JenkinsAgent\\workspace\\Retail-WFM-ESO\\Engines\\Deploy\\nApeExtensionsLibrary\\nApeExtensionsLibrary.wixproj\" (176) on node 1 (default targets).";
      var token = new ProjectBuildStartLine();
      var tokenMatchesLine = token.ProcessLine(lineNumber, testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var expectedRegexMatch = "nApeExtensionsLibrary";
      var actualEventExists = EventHelper.HasSpecificProjectStartEvent(lineNumber, testLine, expectedRegexMatch);
      Assert.IsTrue(actualEventExists);
    }
  }
}
