using Newtonsoft.Json.Linq;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;

namespace VerticalSliceMinimalApi.Test.Domain.Account.ValueObjects;

public class MobileTest
{
    [Fact]
    public void Mobile_Create_ValueObject()
    {
        var mobile = Mobile.Create("1111111111");

        Assert.NotNull(mobile);
        Assert.Equal("1111111111", mobile.Value);
    }

    [Theory]
    [InlineData("1111")]
    [InlineData("111111111111")]
    public void Mobile_Empty_Throws_ArgumentException(string number)
    {
        var exception = Assert.Throws<ArgumentException>(() => Mobile.Create(number));
        Assert.Equal($"No valid mobile number {number}", exception.Message);
    }

}