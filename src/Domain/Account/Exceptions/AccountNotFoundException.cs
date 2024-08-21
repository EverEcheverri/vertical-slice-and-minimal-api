using VerticalSliceMinimalApi.Domain.Account.ValueObjects;
using VerticalSliceMinimalApi.Domain.SharedKernel.Exceptions;

namespace VerticalSliceMinimalApi.Domain.Account.Exceptions;

public class AccountNotFoundException : BusinessException
{
    public AccountNotFoundException(Email email)
        : base($"The account with the email = {email.Value} was not found")
    {
    }

    public AccountNotFoundException(AccountId id)
       : base($"The account with the ID = {id.Value} was not found")
    {
    }
}