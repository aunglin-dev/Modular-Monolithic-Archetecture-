namespace LoginSolution.Shared.Domain.Models.Common;

public class ApiResponseModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public IReadOnlyList<string> Errors { get; set; } = Array.Empty<string>();

    public static ApiResponseModel Success(string message = "Success") => new() { IsSuccess = true, Message = message };
    public static ApiResponseModel Failure(string message, IReadOnlyList<string>? errors = null) => new() { IsSuccess = false, Message = message, Errors = errors ?? Array.Empty<string>() };
}

public sealed class ApiResponseModel<T> : ApiResponseModel
{
    public T? Data { get; set; }
    public static ApiResponseModel<T> Success(T data, string message = "Success") => new() { IsSuccess = true, Message = message, Data = data };
    public new static ApiResponseModel<T> Failure(string message, IReadOnlyList<string>? errors = null) => new() { IsSuccess = false, Message = message, Errors = errors ?? Array.Empty<string>() };
}
