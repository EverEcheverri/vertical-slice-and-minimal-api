using Microsoft.EntityFrameworkCore;
using NSubstitute;
using VerticalSliceMinimalApi.Application.Features.Account.Get;
using VerticalSliceMinimalApi.Domain.Account;
using VerticalSliceMinimalApi.Domain.Account.Exceptions;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;
using VerticalSliceMinimalApi.Persistence;
using VerticalSliceMinimalApi.Test.Data.Account;

namespace VerticalSliceMinimalApi.Test.Application.Features.Account.Create;

public class GetAccountQueryHandlerTest
{
    private readonly IAccountRepository _accountRepositoryMock;
    private readonly DbContextOptions<ApplicationDbContext> _options;
    public GetAccountQueryHandlerTest()
    {
        _accountRepositoryMock = Substitute.For<IAccountRepository>();
    }

    [Fact]
    public async Task GetAccountQueryHandler_Should_Return_Account_When_Email_Exist()
    {
        // Arrange
        var account = AccountMother.Create();

        var email = Email.Create("developer_one@yopmail.com");

        var query = new GetAccountQuery(email);

        _accountRepositoryMock.GetByEmailAsync(email)
            .Returns(account);

        var handler = new GetAccountQueryHandler(_accountRepositoryMock);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(Guid.Parse("b2181377-6a51-446e-afb6-07f1402834e3"), result.Id);
        Assert.Equal("developer_one@yopmail.com", account.Email.Value);
    }

    [Fact]
    public async Task GetAccountQueryHandler_Should_Throw_AccountNotFoundException_When_Email_DoesNot_Exists()
    {
        // Arrange
        var account = AccountMother.Create();

        var email = Email.Create("developer_one@yopmail.com");

        var query = new GetAccountQuery(email);

        _accountRepositoryMock.GetByEmailAsync(email)
            .Returns(null as Entities.Account.Account);

        var handler = new GetAccountQueryHandler(_accountRepositoryMock);

        // Act
        var exception =
        await Assert.ThrowsAsync<AccountNotFoundException>(() => handler.Handle(query, CancellationToken.None));

        // Asserts
        Assert.Equal($"The account with the email = {email.Value} was not found", exception.Message);

        // Assert
        await _accountRepositoryMock.Received().GetByEmailAsync(Arg.Any<Email>());
        _accountRepositoryMock.DidNotReceive().RemoveAsync(Arg.Any<Entities.Account.Account>());
    }
}