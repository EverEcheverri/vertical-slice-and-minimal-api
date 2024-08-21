using MediatR;
using VerticalSliceMinimalApi.Domain.Account.Enums;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;

namespace VerticalSliceMinimalApi.Application.Features.Account.Update;

public record UpdateAccountCommand(
    AccountId AccountId,
    string Email, 
    string UserName,
    string Mobile,
    AccountType AccountType,
    CityId CityId) : IRequest;

public record UpdateAccountRequest(
    string Email,
    string UserName,
    string Mobile,
    int AccountType,
    Guid CityId);