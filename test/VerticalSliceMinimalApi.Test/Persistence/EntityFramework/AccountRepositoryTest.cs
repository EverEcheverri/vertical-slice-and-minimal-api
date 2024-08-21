using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NSubstitute;
using VerticalSliceMinimalApi.Domain.Account;
using VerticalSliceMinimalApi.Domain.Account.Enums;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;
using VerticalSliceMinimalApi.Persistence;
using VerticalSliceMinimalApi.Persistence.EntityFramework;
using VerticalSliceMinimalApi.Test.Data.Account;

namespace VerticalSliceMinimalApi.Test.Persistence.EntityFramework;

public class AccountRepositoryTest
{
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private IPublisher _publisherMock;
    private IAccountRepository _accountRepository;

    public AccountRepositoryTest()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
      .UseInMemoryDatabase(Guid.NewGuid().ToString())
      .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
      .Options;

        _publisherMock = Substitute.For<IPublisher>();
    }

    [Fact]
    public async Task Persistence_AccountRepository_GetByEmailAsync_Returns_Account_WhenExists()
    {
        // Arrange
        var accountOne = AccountMother.Create();
        var accountTwo = AccountMother.Create(id: "140c7396-cb76-45ea-88c5-e709702dd927", email: "developer_two@yopmail.com");

        await using var context = new ApplicationDbContext(_options, _publisherMock);
        await context.Accounts.AddAsync(accountOne);
        await context.Accounts.AddAsync(accountTwo);
        await context.SaveChangesAsync();

        await using var contextAssert = new ApplicationDbContext(_options, _publisherMock);
        _accountRepository = new AccountRepository(contextAssert);

        // Act
        var account = await _accountRepository.GetByEmailAsync(accountOne.Email);

        // Assert File
        Assert.Equal(Guid.Parse("b2181377-6a51-446e-afb6-07f1402834e3"), account.Id.Value);
        Assert.Equal("developer_one@yopmail.com", account.Email.Value);
        Assert.Equal("developer_one", account.UserName.Value);
        Assert.Equal("3110002233", account.Mobile.Value);
        Assert.Equal((AccountType)1, account.AccountType);
        Assert.Equal(Guid.Parse("5ebf0600-c390-4b16-945d-eb0e734cf51c"), account.CityId.Value);
    }

    [Fact]
    public async Task Persistence_AccountRepository_GetByEmailAsync_Returns_Null_WhenDoesNotExists()
    {
        // Arrange
        await using var contextAssert = new ApplicationDbContext(_options, _publisherMock);
        _accountRepository = new AccountRepository(contextAssert);

        // Act
        var account = await _accountRepository.GetByEmailAsync(Email.Create("developer_two@yopmail.com"));

        // Assert File
        Assert.Null(account);
    }

    [Fact]
    public async Task Persistence_AccountRepository_SaveAsync()
    {
        // Arrange
        var account = AccountMother.Create();

        await using var context = new ApplicationDbContext(_options, _publisherMock);
        _accountRepository = new AccountRepository(context);

        // Act
        await _accountRepository.AddAsync(account);
        await context.SaveChangesAsync();

        // Assert Account Created
        await using var contextAssert = new ApplicationDbContext(_options, _publisherMock);
        var created = await contextAssert.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id);

        Assert.NotNull(created);
        Assert.Equal(Guid.Parse("b2181377-6a51-446e-afb6-07f1402834e3"), created.Id.Value);
        Assert.Equal("developer_one@yopmail.com", created.Email.Value);
        Assert.Equal("developer_one", created.UserName.Value);
        Assert.Equal("3110002233", created.Mobile.Value);
        Assert.Equal((AccountType)1, created.AccountType);
        Assert.Equal(Guid.Parse("5ebf0600-c390-4b16-945d-eb0e734cf51c"), created.CityId.Value);
    }

    [Fact]
    public async Task Persistence_AccountRepository_UpdateAsync()
    {
        // Arrange
        var account = AccountMother.Create();

        await using var context = new ApplicationDbContext(_options, _publisherMock);
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();

        account.Update(
            Email.Create("developer_two@yopmail.com"),
            UserName.Create("developer_two"),
            account.Mobile,
            account.AccountType,
            account.CityId);

        await using var contextUpdate = new ApplicationDbContext(_options, _publisherMock);
        _accountRepository = new AccountRepository(contextUpdate);

        // Act
        _accountRepository.UpdateAsync(account);
        await context.SaveChangesAsync();

        // Assert Account Created
        await using var contextAssert = new ApplicationDbContext(_options, _publisherMock);
        var created = await contextAssert.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id);

        Assert.NotNull(created);
        Assert.Equal(Guid.Parse("b2181377-6a51-446e-afb6-07f1402834e3"), created.Id.Value);
        Assert.Equal("developer_two@yopmail.com", created.Email.Value);
        Assert.Equal("developer_two", created.UserName.Value);
        Assert.Equal("3110002233", created.Mobile.Value);
        Assert.Equal((AccountType)1, created.AccountType);
        Assert.Equal(Guid.Parse("5ebf0600-c390-4b16-945d-eb0e734cf51c"), created.CityId.Value);
    }

    [Fact]
    public async Task Persistence_AccountRepository_RemoveAsync()
    {
        // Arrange
        var account = AccountMother.Create();

        await using var context = new ApplicationDbContext(_options, _publisherMock);
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();

        _accountRepository = new AccountRepository(context);

        // Act
        _accountRepository.RemoveAsync(account);
        await context.SaveChangesAsync();

        // Assert Account Created
        await using var contextAssert = new ApplicationDbContext(_options, _publisherMock);
        var removed = await contextAssert.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id);

        Assert.Null(removed);
    }
}
