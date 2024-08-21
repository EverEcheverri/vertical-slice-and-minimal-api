using Microsoft.EntityFrameworkCore;
using NSubstitute;
using VerticalSliceMinimalApi.Application.Features.Account.Create;
using VerticalSliceMinimalApi.Application.Features.Account.Get;
using VerticalSliceMinimalApi.Application.Features.Account.Update;
using VerticalSliceMinimalApi.Domain.Account;
using VerticalSliceMinimalApi.Domain.Account.Enums;
using VerticalSliceMinimalApi.Domain.Account.Exceptions;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;
using VerticalSliceMinimalApi.Entities.Account;
using VerticalSliceMinimalApi.Persistence;
using VerticalSliceMinimalApi.Test.Data.Account;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VerticalSliceMinimalApi.Test.Application.Features.Account.Create;

public class UpdateAccountCommandHandlerTest
{
    private readonly IAccountRepository _accountRepositoryMock;
    private readonly DbContextOptions<ApplicationDbContext> _options;
    public UpdateAccountCommandHandlerTest()
    {
        _accountRepositoryMock = Substitute.For<IAccountRepository>();
    }

    [Fact]
    public async Task UpdateAccountCommandHandler_Should_Update_Account_When_Email_Exist()
    {
        // Arrange
        var account = AccountMother.Create();

        var command =
            new UpdateAccountCommand
            (
                account.Id,
                "developer_one@yopmail.com",
                "developer_two",
                "3111111111",
                AccountType.Buyer,
                account.CityId
            );

        _accountRepositoryMock.GetByIdAsync(account.Id)
            .Returns(account);

        Entities.Account.Account? accountUpdated = null;
        _accountRepositoryMock.When(a => a.UpdateAsync(Arg.Any<Entities.Account.Account>()))
            .Do(call => accountUpdated = call.ArgAt<Entities.Account.Account>(0));

        var unitOfWork = Substitute.For<IUnitOfWork>();

        var handler = new UpdateAccountCommandHandler(_accountRepositoryMock, unitOfWork);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(accountUpdated);
        Assert.Equal("developer_two", accountUpdated.UserName.Value);
        Assert.Equal("developer_one@yopmail.com", account.Email.Value);
        Assert.Equal("3111111111", account.Mobile.Value);

        await _accountRepositoryMock.Received().GetByIdAsync(Arg.Any<AccountId>());
        _accountRepositoryMock.Received().UpdateAsync(Arg.Any<Entities.Account.Account>());
        await unitOfWork.Received().SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task UpdateAccountCommandHandler_Should_Throw_AccountNotFoundException_When_Email_DoesNot_Exists()
    {
        // Arrange
        var accountId = AccountId.Create(Guid.Parse("b2181377-6a51-446e-afb6-07f1402834e3"));

        var command =
             new UpdateAccountCommand
             (
                 accountId,
                 "developer_one@yopmail.com",
                 "developer_two",
                 "3111111111",
                 AccountType.Buyer,
                 new CityId(Guid.Parse("b2181377-6a51-446e-afb6-07f1402834e3"))
             );

        _accountRepositoryMock.GetByIdAsync(command.AccountId)
            .Returns(null as Entities.Account.Account);

        var unitOfWork = Substitute.For<IUnitOfWork>();

        var handler = new UpdateAccountCommandHandler(_accountRepositoryMock, unitOfWork);

        // Act
        var exception =
        await Assert.ThrowsAsync<AccountNotFoundException>(() => handler.Handle(command, CancellationToken.None));

        // Asserts
        Assert.Equal($"The account with the ID = {command.AccountId.Value} was not found", exception.Message);

        // Assert
        await _accountRepositoryMock.Received().GetByIdAsync(Arg.Any<AccountId>());
        _accountRepositoryMock.DidNotReceive().UpdateAsync(Arg.Any<Entities.Account.Account>());
        await unitOfWork.DidNotReceive().SaveChangesAsync(CancellationToken.None);
    }
}