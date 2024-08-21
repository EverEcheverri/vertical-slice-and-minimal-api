namespace VerticalSliceMinimalApi.Domain.Account.ValueObjects;

public record UserName
{
    private UserName(string value) => Value = value;

    public string Value { get; init; }

    public static UserName Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("User name is null or empty");
        }
        return new UserName(value);
    }
}
