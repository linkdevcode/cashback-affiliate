namespace Cashback.Api.Models;

/// <summary>
/// Standard API response wrapper.
/// </summary>
public sealed class ApiResponse<T>
{
    /// <summary>
    /// Indicates whether the request succeeded.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Optional response message.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Response payload.
    /// </summary>
    public T? Data { get; init; }

    /// <summary>
    /// Validation or error details.
    /// </summary>
    public IReadOnlyList<string>? Errors { get; init; }

    /// <summary>
    /// Creates a successful response with data.
    /// </summary>
    public static ApiResponse<T> Ok(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Creates a failed response with an error message.
    /// </summary>
    public static ApiResponse<T> Fail(string message, IReadOnlyList<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
}
