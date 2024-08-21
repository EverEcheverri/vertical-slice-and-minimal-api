using VerticalSliceMinimalApi.Domain.Account.ValueObjects;

namespace VerticalSliceMinimalApi.Test.Domain.Account.ValueObjects;

public class AccountIdTest
{
    [Fact]
    public void AccountId_Should_Have_Valid_Guid_Value()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var accountId = AccountId.Create(guid);

        // Assert
        Assert.Equal(guid, accountId.Value);
    }

    [Fact]
    public void AccountId_Throws_ArgumentException()
    {
        // Arrange
        Guid nullGuid = Guid.Empty;

        // Act
        var exception = Assert.Throws<ArgumentException>(() => AccountId.Create(nullGuid));

        // Assert
        Assert.Equal("AccountId cannot be default Guid or empty", exception.Message);
    }
}
