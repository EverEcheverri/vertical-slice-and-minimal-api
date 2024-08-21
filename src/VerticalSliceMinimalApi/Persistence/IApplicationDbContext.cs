using Microsoft.EntityFrameworkCore;
using VerticalSliceMinimalApi.Entities.Account;

namespace VerticalSliceMinimalApi.Persistence;

public interface IApplicationDbContext
{
    DbSet<Account> Accounts { get; set; }
}
