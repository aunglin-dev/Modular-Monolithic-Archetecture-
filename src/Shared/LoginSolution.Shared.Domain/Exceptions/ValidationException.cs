namespace LoginSolution.Shared.Domain.Exceptions;

public sealed class ValidationException : Exception { public IReadOnlyList<string> Errors { get; } public ValidationException(string message) : base(message) => Errors = new[] { message }; public ValidationException(IReadOnlyList<string> errors) : base("Validation failed.") => Errors = errors; }
