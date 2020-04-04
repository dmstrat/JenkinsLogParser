using JenkinsLogParser.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JenkinsLogParser.Unit.Tests.Tokens
{
  [TestClass]
  public class WarningLineTests
  {
    [TestMethod]
    public void WhenWarningLineRawProvidedWeGetMSB3276Back()
    {
      var testLine = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Professional\\MSBuild\\Current\\Bin\\Microsoft.Common.CurrentVersion.targets(2106,5): warning MSB3276: Found conflicts between different versions of the same dependent assembly. Please set the \"AutoGenerateBindingRedirects\" property to true in the project file.";
      var token = new WarningLine();
      var tokenMatchesLine = token.ProcessLine(0, testLine);
      Assert.IsNotNull(token);
      Assert.IsTrue(tokenMatchesLine, "Token didn't match line as expected:" + testLine);
      var expectedLine = "MSB3276";
      var actualLine = token.GetLine();
      Assert.AreEqual(expectedLine, actualLine);
    }
  }
}
