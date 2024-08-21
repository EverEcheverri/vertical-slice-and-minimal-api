using VerticalSliceMinimalApi.Domain;
using VerticalSliceMinimalApi.Domain.Account.Enums;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;

namespace VerticalSliceMinimalApi.Entities.Account;

public class Account : Entity
{
    public Account(AccountId id, Email email, UserName userName, Mobile mobile, AccountType accountType, CityId cityId)
    {
        Id = id;
        Email = email;
        UserName = userName;
        Mobile = mobile;
        AccountType = accountType;
        CityId = cityId;
    }
    public AccountId Id { get; private set; }
    public Email Email { get; private set; }
    public UserName UserName { get; private set; }
    public Mobile Mobile { get; private set; }
    public AccountType AccountType { get; private set; }
    public CityId CityId { get; private set; }

    public void Update(Email email, UserName userName, Mobile mobile, AccountType accountType, CityId cityId)
    {
        Email = email;
        UserName = userName;
        Mobile = mobile;
        AccountType = accountType;
        CityId = cityId;
    }
}