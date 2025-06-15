namespace Cookbook.Communication.Responses;

public class ErrorResponse
{
    public IList<string> Errors { get; set; }

    public ErrorResponse(IList<string> errors) => Errors = errors;

    public ErrorResponse(string error) => Errors = [error];
}
