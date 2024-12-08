namespace SigmaSoftware.Application.Common.Exceptions;

public class ValidationException(IEnumerable<ValidationError> errors) : Exception
{
    public IEnumerable<ValidationError> Errors { get; } = errors;
}

public sealed record ValidationError(string PropertyName, string ErrorMessage);