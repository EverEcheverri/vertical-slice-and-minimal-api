using System.Text.RegularExpressions;

namespace VerticalSliceMinimalApi.Domain.Account.ValueObjects;

public record Email
{
    private Email(string value) => Value = value;

    public string Value { get; init; }

    public static Email Create(string value)
    {
        if (!IsValidEmail(value))
        {
            throw new ArgumentException($"No valid email {value}");
        }
        return new Email(value);
    }

    private static bool IsValidEmail(string value)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(value, pattern);
    }
}