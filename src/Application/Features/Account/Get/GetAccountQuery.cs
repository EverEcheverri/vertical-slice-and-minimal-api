using MediatR;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;

namespace VerticalSliceMinimalApi.Application.Features.Account.Get;


public record GetAccountQuery(Email email) : IRequest<AccountResponse>;

public record AccountResponse(
    Guid Id,
    string Email,
    string UserName,
    string Mobile,
    int AccountType,
    Guid CityId);
