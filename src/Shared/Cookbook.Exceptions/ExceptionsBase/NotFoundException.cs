namespace Cookbook.Exceptions.ExceptionsBase;

public class NotFoundException(string message) : CookbookException(message);
