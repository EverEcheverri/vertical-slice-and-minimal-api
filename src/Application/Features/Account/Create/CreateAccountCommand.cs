using MediatR;
using VerticalSliceMinimalApi.Domain.Account.Enums;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;

namespace VerticalSliceMinimalApi.Application.Features.Account.Create;

public record CreateAccountCommand(
    Email Email,
    UserName UserName,
    Mobile Mobile,
    AccountType AccountType,
    Guid CityId) : IRequest;

public record CreateAccountRequest(
    string Email,
    string UserName,
    string Mobile,
    int AccountType,
    Guid CityId);