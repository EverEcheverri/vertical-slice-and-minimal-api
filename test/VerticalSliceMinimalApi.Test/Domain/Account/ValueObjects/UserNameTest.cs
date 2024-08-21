using VerticalSliceMinimalApi.Domain.Account.ValueObjects;

namespace VerticalSliceMinimalApi.Test.Domain.Account.ValueObjects;

public class UserNameTest
{
    [Fact]
    public void UserName_Create_ValueObject()
    {
        var userName = UserName.Create("developer_one");

        Assert.NotNull(userName);
        Assert.Equal("developer_one", userName.Value);
    }

    [Fact]
    public void UserName_Empty_Throws_ArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => UserName.Create(string.Empty));
        Assert.Equal("User name is null or empty", exception.Message);
    }
}
