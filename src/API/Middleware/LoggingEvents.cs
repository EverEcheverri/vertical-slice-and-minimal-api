namespace VerticalSliceMinimalApi.API.Middleware;

public class LoggingEvents
{
    public static readonly int Unknown = 000;

    public static readonly int ArgumentException = 100;
    public static readonly int AccountNotFoundException = 101;
    public static readonly int AccountAlreadyExistsException = 102;


    public static readonly int GeneralValidationError = 199;
}
