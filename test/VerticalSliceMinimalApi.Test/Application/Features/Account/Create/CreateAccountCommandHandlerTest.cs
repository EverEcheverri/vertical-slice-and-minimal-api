using NSubstitute;
using VerticalSliceMinimalApi.Application.Features.Account.Create;
using VerticalSliceMinimalApi.Domain.Account;
using VerticalSliceMinimalApi.Domain.Account.Enums;
using VerticalSliceMinimalApi.Domain.Account.Exceptions;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;
using VerticalSliceMinimalApi.Persistence;
using VerticalSliceMinimalApi.Test.Data.Account;

namespace VerticalSliceMinimalApi.Test.Application.Features.Account.Create;

public class CreateAccountCommandHandlerTests
{
    private readonly IAccountRepository _accountRepositoryMock;
    public CreateAccountCommandHandlerTests()
    {
        _accountRepositoryMock = Substitute.For<IAccountRepository>();
    }

    [Fact]
    public async Task CreateAccountCommandHandler_Should_Create_Account_When_Email_Doesnt_Exist()
    {
        // Arrange
        var command =
            new CreateAccountCommand
            (
                Email.Create("test@email.com"),
                UserName.Create("Test User"),
                Mobile.Create("1234567890"),
                AccountType.Buyer,
                Guid.NewGuid()
            );

        _accountRepositoryMock.GetByEmailAsync(command.Email)
            .Returns(null as Entities.Account.Account);

        var unitOfWork = Substitute.For<IUnitOfWork>();

        var handler = new CreateAccountCommandHandler(_accountRepositoryMock, unitOfWork);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _accountRepositoryMock.Received().AddAsync(Arg.Any<Entities.Account.Account>());
        await unitOfWork.Received().SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task CreateAccountCommandHandler_Should_Throw_AccountAlreadyExistsException_When_Email_Exists()
    {
        // Arrange
        var command =
           new CreateAccountCommand
           (
               Email.Create("test@email.com"),
               UserName.Create("Test User"),
               Mobile.Create("1234567890"),
               AccountType.Buyer,
               Guid.NewGuid()
           );

        var existingAccount = AccountMother.Create();

        _accountRepositoryMock.GetByEmailAsync(command.Email)
            .Returns(existingAccount);

        var unitOfWork = Substitute.For<IUnitOfWork>();

        var handler = new CreateAccountCommandHandler(_accountRepositoryMock, unitOfWork);

        // Act
        var exception =
        await Assert.ThrowsAsync<AccountAlreadyExistsException>(() => handler.Handle(command, CancellationToken.None));

        // Asserts
        Assert.Equal($"The account with the email = {command.Email.Value} already exists", exception.Message);

        // Assert
        await _accountRepositoryMock.DidNotReceive().AddAsync(Arg.Any<Entities.Account.Account>());
        await unitOfWork.DidNotReceive().SaveChangesAsync(CancellationToken.None);
    }
}