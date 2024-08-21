using VerticalSliceMinimalApi.Domain.Account.ValueObjects;

namespace VerticalSliceMinimalApi.Test.Domain.Account.ValueObjects;

public class CityIdTest
{
    [Fact]
    public void CityId_Should_Have_Valid_Guid_Value()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var cityId = new CityId(guid);

        // Assert
        Assert.Equal(guid, cityId.Value);
    }
}
