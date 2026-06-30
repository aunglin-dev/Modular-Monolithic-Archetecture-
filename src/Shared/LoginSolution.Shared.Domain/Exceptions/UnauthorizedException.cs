namespace LoginSolution.Shared.Domain.Exceptions;

public sealed class UnauthorizedException : Exception { public UnauthorizedException(string message) : base(message) { } }
