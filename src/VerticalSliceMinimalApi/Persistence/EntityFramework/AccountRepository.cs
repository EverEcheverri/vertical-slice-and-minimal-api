using Microsoft.EntityFrameworkCore;
using VerticalSliceMinimalApi.Domain.Account;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;
using VerticalSliceMinimalApi.Entities.Account;

namespace VerticalSliceMinimalApi.Persistence.EntityFramework;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Account account)
    {
       await  _context.Accounts.AddAsync(account);
    }

    public async Task<Account?> GetByEmailAsync(Email email)
    {
        return await _context.Accounts
           .SingleOrDefaultAsync(p => p.Email == email);
    }

    public async Task<Account?> GetByIdAsync(AccountId id)
    {
        return await _context.Accounts
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public void RemoveAsync(Account account)
    {
        _context.Accounts.Remove(account);
    }

    public void UpdateAsync(Account account)
    {
        _context.Accounts.Update(account);
    }
}
