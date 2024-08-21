using NSubstitute;
using VerticalSliceMinimalApi.Application.Features.Account.Delete;
using VerticalSliceMinimalApi.Domain.Account;
using VerticalSliceMinimalApi.Domain.Account.Exceptions;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;
using VerticalSliceMinimalApi.Persistence;
using VerticalSliceMinimalApi.Test.Data.Account;

namespace VerticalSliceMinimalApi.Test.Application.Features.Account.Create;

public class DeleteAccountCommandHandlerTests
{
    private readonly IAccountRepository _accountRepositoryMock;
    public DeleteAccountCommandHandlerTests()
    {
        _accountRepositoryMock = Substitute.For<IAccountRepository>();
    }

    [Fact]
    public async Task DeleteAccountCommandHandler_Should_Delete_Account_When_Email_Exist()
    {
        // Arrange
        var command =
            new DeleteAccountCommand
            (
                AccountId.Create(Guid.Parse("f7aa85fc-147f-4ef7-9d0a-4de3eacdd1f8"))
            );

        var existingAccount = AccountMother.Create();

        _accountRepositoryMock.GetByIdAsync(command.AccountId)
            .Returns(existingAccount);

        var unitOfWork = Substitute.For<IUnitOfWork>();

        var handler = new DeleteAccountCommandHandler(_accountRepositoryMock, unitOfWork);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _accountRepositoryMock.Received().GetByIdAsync(Arg.Any<AccountId>());
        _accountRepositoryMock.Received().RemoveAsync(Arg.Any<Entities.Account.Account>());
        await unitOfWork.Received().SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task DeleteAccountCommandHandler_Should_Throw_AccountNotFoundException_When_Email_DoesNot_Exists()
    {
        // Arrange
        var command =
            new DeleteAccountCommand
            (
                AccountId.Create(Guid.Parse("f7aa85fc-147f-4ef7-9d0a-4de3eacdd1f8"))
            );

        _accountRepositoryMock.GetByIdAsync(command.AccountId)
            .Returns(null as Entities.Account.Account);

        var unitOfWork = Substitute.For<IUnitOfWork>();

        var handler = new DeleteAccountCommandHandler(_accountRepositoryMock, unitOfWork);

        // Act
        var exception =
        await Assert.ThrowsAsync<AccountNotFoundException>(() => handler.Handle(command, CancellationToken.None));

        // Asserts
        Assert.Equal($"The account with the ID = {command.AccountId.Value} was not found", exception.Message);

        // Assert
        await _accountRepositoryMock.Received().GetByIdAsync(Arg.Any<AccountId>());
        _accountRepositoryMock.DidNotReceive().RemoveAsync(Arg.Any<Entities.Account.Account>());
        await unitOfWork.DidNotReceive().SaveChangesAsync(CancellationToken.None);
    }
}