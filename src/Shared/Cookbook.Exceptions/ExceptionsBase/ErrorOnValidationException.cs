namespace Cookbook.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : CookbookException
{
    public IList<string> ErrorMessages { get; set; }

    public ErrorOnValidationException(IList<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}
