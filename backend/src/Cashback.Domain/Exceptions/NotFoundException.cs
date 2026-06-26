namespace Cashback.Domain.Exceptions;

/// <summary>
/// Exception thrown when a requested resource cannot be found.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance with a default message.
    /// </summary>
    public NotFoundException()
        : base("Resource not found.")
    {
    }

    /// <summary>
    /// Initializes a new instance with a custom message.
    /// </summary>
    public NotFoundException(string message)
        : base(message)
    {
    }
}
