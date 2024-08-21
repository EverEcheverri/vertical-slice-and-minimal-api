using MediatR;
using Microsoft.AspNetCore.Mvc;
using VerticalSliceMinimalApi.Application.Features.Account.Create;
using VerticalSliceMinimalApi.Application.Features.Account.Delete;
using VerticalSliceMinimalApi.Application.Features.Account.Get;
using VerticalSliceMinimalApi.Application.Features.Account.Update;
using VerticalSliceMinimalApi.Domain.Account.Enums;
using VerticalSliceMinimalApi.Domain.Account.Exceptions;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;

namespace VerticalSliceMinimalApi.API.Endpoints;

public static class AccountsEndpoints
{
    public static void MapAccountsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/accounts");

        group.MapPost("", HandleCreateAccountAsync)
          .Produces(400)
          .Produces(500);

        group.MapGet("{email}", HandleGetByEmailAsync)
             .Produces<AccountResponse>(200)
             .Produces(400)
             .Produces(500);

        group.MapPatch("{id:guid}", HandleUpdateAccountAsync)
             .Produces(400)
             .Produces(500);

        group.MapDelete("{id:guid}", HandleDeleteAccountAsync)
             .Produces(400)
             .Produces(500);
    }

    public static async Task<IResult> HandleCreateAccountAsync(CreateAccountRequest request, ISender sender)
    {
        var command = new CreateAccountCommand(
            Email.Create(request.Email),
            UserName.Create(request.UserName),
            Mobile.Create(request.Mobile),
            (AccountType)request.AccountType,
            request.CityId
        );

        await sender.Send(command);
        return Results.Created();
    }
    public static async Task<IResult> HandleGetByEmailAsync(string email, ISender sender)
    {
        try
        {
            var result = await sender.Send(new GetAccountQuery(Email.Create(email)));
            return Results.Ok(result);
        }
        catch (AccountNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
    }

    public static async Task<IResult> HandleUpdateAccountAsync(Guid id, UpdateAccountRequest request, ISender sender)
    {
        var command = new UpdateAccountCommand(
            AccountId.Create(id),
            request.Email,
            request.UserName,
            request.Mobile,
            (AccountType)request.AccountType,
            new CityId(request.CityId)
        );

        await sender.Send(command);
        return Results.NoContent();
    }

    public static async Task<IResult> HandleDeleteAccountAsync(Guid id, ISender sender)
    {
        try
        {
            await sender.Send(new DeleteAccountCommand(AccountId.Create(id)));
            return Results.NoContent();
        }
        catch (AccountNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
    }
}
