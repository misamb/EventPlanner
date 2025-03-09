using WebApp;

namespace Tests.UnitTests;

public class HelperClassTests
{
    [Fact]
    public void TestMethod_IsInFuture()
    {
        var pastTime = DateTime.Now.AddDays(-1);
        var futureTime = DateTime.Now.AddDays(2);
        
        Assert.True(Helpers.IsInFuture(futureTime));
        Assert.False(Helpers.IsInFuture(pastTime));
    }
}