using System.Text.RegularExpressions;

namespace VerticalSliceMinimalApi.Domain.Account.ValueObjects;

public record AccountId
{
    private AccountId(Guid value) => Value = value;

    public Guid Value { get; init; }

    public static AccountId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("AccountId cannot be default Guid or empty");
        }

        return new AccountId(value);
    }
}
