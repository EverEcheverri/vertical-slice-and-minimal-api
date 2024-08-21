using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using VerticalSliceMinimalApi.API.Endpoints;
using VerticalSliceMinimalApi.Application.Features.Account.Create;
using VerticalSliceMinimalApi.Application.Features.Account.Delete;
using VerticalSliceMinimalApi.Application.Features.Account.Get;
using VerticalSliceMinimalApi.Application.Features.Account.Update;

public class EndpointMappingTests
{
    private readonly ISender _mockSender;
    public EndpointMappingTests()
    {
        _mockSender = Substitute.For<ISender>();
    }

    [Fact]
    public async Task HandleCreateAccountAsync_ShouldReturn_Created()
    {
        // Arrange
        var createAccountRequest = new CreateAccountRequest(
            "developer_one@yopmail.com",
            "developer_one",
            "3110002233",
            1,
            Guid.NewGuid()
        );

        // Act
        var result = await AccountsEndpoints.HandleCreateAccountAsync(createAccountRequest, _mockSender);

        // Assert
        Assert.IsType<Created>(result);

        // Verify that the command was sent
        await _mockSender.Received(1).Send(Arg.Any<CreateAccountCommand>());
    }

    [Fact]
    public async Task HandleGetByEmailAsync_ShouldReturn_Ok()
    {
        // Act
        var result = await AccountsEndpoints.HandleGetByEmailAsync("developer_one@yopmail.com", _mockSender);

        // Assert
        Assert.IsType<Ok>(result);

        // Verify that the command was sent
        await _mockSender.Received(1).Send(Arg.Any<GetAccountQuery>());
    }

    [Fact]
    public async Task HandleUpdateAccountAsync_ShouldReturn_NoContent()
    {
        // Arrange
        Guid id = Guid.Parse("b2181377-6a51-446e-afb6-07f1402834e3");

        var updateAccountRequest = new UpdateAccountRequest(
            "developer_one@yopmail.com",
            "developer_one",
            "3110002233",
            1,
            Guid.NewGuid()
        );

        // Act
        var result = await AccountsEndpoints.HandleUpdateAccountAsync(id, updateAccountRequest, _mockSender);

        // Assert
        Assert.IsType<NoContent>(result);

        // Verify that the command was sent
        await _mockSender.Received(1).Send(Arg.Any<UpdateAccountCommand>());
    }

    [Fact]
    public async Task HandleDeleteAccountAsync_ShouldReturn_NoContent()
    {
        // Arrange
        Guid id = Guid.Parse("b2181377-6a51-446e-afb6-07f1402834e3");

        // Act
        var result = await AccountsEndpoints.HandleDeleteAccountAsync(id, _mockSender);

        // Assert
        Assert.IsType<NoContent>(result);

        // Verify that the command was sent
        await _mockSender.Received(1).Send(Arg.Any<DeleteAccountCommand>());
    }
}