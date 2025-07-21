namespace Cookbook.Exceptions.ExceptionsBase;

public class ErrorOnValidationException(IList<string> errorMessages) : CookbookException(string.Empty)
{
    public IList<string> ErrorMessages = errorMessages;
}
