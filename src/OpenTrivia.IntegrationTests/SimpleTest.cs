namespace OpenTrivia.IntegrationTests;

[TestClass]
public class SimpleTest
{
    [TestMethod]
    public void RevisionNumberTest()
    {
        // We need at least 1 simple test to avoid MS Test failing on "Zero tests run" error in our CI/CD pipeline.
        var revision = ApiConstants.ApiRevision;
        Assert.AreEqual(0, revision);
    }
}
