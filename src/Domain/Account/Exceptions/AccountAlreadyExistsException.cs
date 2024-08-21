using VerticalSliceMinimalApi.Domain.Account.ValueObjects;
using VerticalSliceMinimalApi.Domain.SharedKernel.Exceptions;

namespace VerticalSliceMinimalApi.Domain.Account.Exceptions
{
    public class AccountAlreadyExistsException : BusinessException
    {
        public AccountAlreadyExistsException(Email email)
            : base($"The account with the email = {email.Value} already exists")
        {
        }
    }
}