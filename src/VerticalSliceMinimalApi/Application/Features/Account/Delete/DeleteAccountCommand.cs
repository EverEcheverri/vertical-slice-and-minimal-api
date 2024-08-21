using MediatR;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;

namespace VerticalSliceMinimalApi.Application.Features.Account.Delete;

public record DeleteAccountCommand(AccountId AccountId) : IRequest;