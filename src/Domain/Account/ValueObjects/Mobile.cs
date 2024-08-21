using System.Text.RegularExpressions;

namespace VerticalSliceMinimalApi.Domain.Account.ValueObjects;

public record Mobile
{
    private Mobile(string value) => Value = value;

    public string Value { get; init; }

    public static Mobile Create(string value)
    {
        if (!IsValidPhoneNumber(value))
        {
            throw new ArgumentException($"No valid mobile number {value}");
        }
        return new Mobile(value);
    }
    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        string pattern = @"^\d{10}$";
        return Regex.IsMatch(phoneNumber, pattern);
    }
}