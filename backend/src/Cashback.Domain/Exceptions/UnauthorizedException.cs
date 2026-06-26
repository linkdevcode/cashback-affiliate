namespace Cashback.Domain.Exceptions;

/// <summary>
/// Exception thrown when authentication fails or credentials are invalid.
/// </summary>
public class UnauthorizedException : Exception
{
    /// <summary>
    /// Initializes a new instance with a default message.
    /// </summary>
    public UnauthorizedException()
        : base("Authentication failed.")
    {
    }

    /// <summary>
    /// Initializes a new instance with a custom message.
    /// </summary>
    public UnauthorizedException(string message)
        : base(message)
    {
    }
}
