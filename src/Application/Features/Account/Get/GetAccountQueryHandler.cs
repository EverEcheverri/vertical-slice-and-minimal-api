using MediatR;
using VerticalSliceMinimalApi.Domain.Account;
using VerticalSliceMinimalApi.Domain.Account.Exceptions;

namespace VerticalSliceMinimalApi.Application.Features.Account.Get;

public sealed class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, AccountResponse>
{
    private readonly IAccountRepository _accountRepository;
    public GetAccountQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountResponse> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByEmailAsync(request.email) 
            ?? throw new AccountNotFoundException(request.email);        

        return new AccountResponse(
                account.Id.Value,
                account.Email.Value,
                account.UserName.Value,
                account.Mobile.Value,
                (int)account.AccountType,
                account.CityId.Value);
    }
}
