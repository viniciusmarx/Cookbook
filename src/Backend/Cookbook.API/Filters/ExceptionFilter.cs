using Cookbook.Communication.Responses;
using Cookbook.Exceptions;
using Cookbook.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Cookbook.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CookbookException)
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnknowException(context);
        }
    }

    private static void HandleProjectException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ErrorOnValidationException validationException:
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new BadRequestObjectResult(new ErrorResponse(validationException.ErrorMessages));
                break;

            case InvalidLoginException loginException:
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = new UnauthorizedObjectResult(new ErrorResponse(loginException.Message));
                break;

            case NotFoundException notFoundException:
                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Result = new NotFoundObjectResult(new ErrorResponse(notFoundException.Message));
                break;
        }
    }

    private static void ThrowUnknowException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ErrorResponse(ResourceMessagesException.UNKNOWN_ERROR));
    }
}