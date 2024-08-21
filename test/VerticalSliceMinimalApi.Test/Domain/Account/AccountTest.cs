using VerticalSliceMinimalApi.Domain.Account.Enums;
using VerticalSliceMinimalApi.Test.Data.Account;

namespace VerticalSliceMinimalApi.Test.Domain.Account
{
    public class AccountTest
    {
        [Fact]
        public void Account_Builds_Successfully()
        {
            // Act
            var account = AccountMother.Create();

            // Assert
            Assert.NotNull(account);
            Assert.Equal(Guid.Parse("b2181377-6a51-446e-afb6-07f1402834e3"), account.Id.Value);
            Assert.Equal("developer_one@yopmail.com", account.Email.Value);
            Assert.Equal("developer_one", account.UserName.Value);
            Assert.Equal("3110002233", account.Mobile.Value);
            Assert.Equal((AccountType)1, account.AccountType);
            Assert.Equal(Guid.Parse("5ebf0600-c390-4b16-945d-eb0e734cf51c"), account.CityId.Value);
        }
    }
}
