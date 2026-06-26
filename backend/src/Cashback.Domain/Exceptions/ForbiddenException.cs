namespace Cashback.Domain.Exceptions;

/// <summary>
/// Exception thrown when the user is authenticated but not permitted to perform the action.
/// </summary>
public class ForbiddenException : Exception
{
    /// <summary>
    /// Initializes a new instance with a default message.
    /// </summary>
    public ForbiddenException()
        : base("Access denied.")
    {
    }

    /// <summary>
    /// Initializes a new instance with a custom message.
    /// </summary>
    public ForbiddenException(string message)
        : base(message)
    {
    }
}
