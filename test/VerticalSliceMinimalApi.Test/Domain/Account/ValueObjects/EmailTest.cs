using VerticalSliceMinimalApi.Domain.Account.ValueObjects;

namespace VerticalSliceMinimalApi.Test.Domain.Account.ValueObjects;

public class EmailTest
{
    [Fact]
    public void Email_Create_ValueObject()
    {
        var email = Email.Create("developer_one@yopmail.com");

        Assert.NotNull(email);
        Assert.Equal("developer_one@yopmail.com", email.Value);
    }

    [Theory]
    [InlineData("developer_one")]
    [InlineData("developer_one@yopmail")]
    [InlineData("developer_one_@@yopmail.com")]
    public void Email_Throws_ArgumentException(string email)
    {
        var exception = Assert.Throws<ArgumentException>(() => Email.Create(email));
        Assert.Equal($"No valid email {email}", exception.Message);
    }
}