using VerticalSliceMinimalApi.Domain.Account.ValueObjects;

namespace VerticalSliceMinimalApi.Domain.Account;

public interface IAccountRepository
{
    Task AddAsync(Entities.Account.Account account);
    Task<Entities.Account.Account?> GetByIdAsync(AccountId id);
    Task<Entities.Account.Account?> GetByEmailAsync(Email email);
    void UpdateAsync(Entities.Account.Account proaccountduct);
    void RemoveAsync(Entities.Account.Account account);
}
