
using VerticalSliceMinimalApi.Domain.Account.Enums;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;

namespace VerticalSliceMinimalApi.Test.Data.Account;

public static class AccountMother
{
    public static Entities.Account.Account Create(
    string id = "b2181377-6a51-446e-afb6-07f1402834e3",
    string email = "developer_one@yopmail.com",
    string userName = "developer_one",
    string mobile = "3110002233",
    int accountType = 1,
    string cityId = "5ebf0600-c390-4b16-945d-eb0e734cf51c")
    {
        return new Entities.Account.Account(
          AccountId.Create(Guid.Parse(id)),
          Email.Create(email),
          UserName.Create(userName),
          Mobile.Create(mobile),
          (AccountType)accountType,
          new CityId(Guid.Parse(cityId)));
    }
}
