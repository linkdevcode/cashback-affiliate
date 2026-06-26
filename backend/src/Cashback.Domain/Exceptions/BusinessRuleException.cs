namespace Cashback.Domain.Exceptions;

/// <summary>
/// Exception thrown when a business rule prevents the operation from completing.
/// </summary>
public class BusinessRuleException : Exception
{
    /// <summary>
    /// Initializes a new instance of the business rule exception.
    /// </summary>
    public BusinessRuleException(string message)
        : base(message)
    {
    }
}
