using JenkinsLogParser.Tokens;
using JenkinsLogParser.Unit.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JenkinsLogParser.Unit.Tests.Tokens
{
  [TestClass]
  public class WarningLineTests
  {
    [ClassInitialize]
    public static void ClassInitialize(TestContext testContext)
    {
      var testDelegateHelper = new TestDelegateHelper();
      var testDelegateHandlers = testDelegateHelper.BuildTestDelegatesDictionary();
    }

    [TestMethod]
    public void WhenWarningLineRawProvidedWeGetMSB3276Back()
    {
      var lineNumber = 17;
      var testLine = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Professional\\MSBuild\\Current\\Bin\\Microsoft.Common.CurrentVersion.targets(2106,5): warning MSB3276: Found conflicts between different versions of the same dependent assembly. Please set the \"AutoGenerateBindingRedirects\" property to true in the project file.";
      var token = new WarningLine();
      var tokenMatchesLine = token.ProcessLine(17, testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var expectedRegexMatch = "MSB3276";
      var actualEventExists = EventHelper.HasSpecificWarningAddedEvent(lineNumber, testLine, expectedRegexMatch);
      Assert.IsTrue(actualEventExists);
    }
  }
}
