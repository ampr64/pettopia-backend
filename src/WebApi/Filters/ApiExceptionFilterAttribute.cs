﻿using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            Action<ExceptionContext> handler = context.Exception switch
            {
                AuthenticationFailedException ex => HandleAuthenticationFailedException,
                UnprocessableEntityException ex => HandleUnprocessableEntityException,
                _ => HandleUnknownException
            };

            handler.Invoke(context);
            context.ExceptionHandled = true;

            base.OnException(context);
        }

        private static void HandleAuthenticationFailedException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Detail = context.Exception.Message,
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
            };

            context.Result = new ObjectResult(details);
        }

        private static void HandleUnprocessableEntityException(ExceptionContext context)
        {
            var details = new
            {
                Detail = context.Exception.Message,
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Unprocessable entity",
                Type = "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2",
                ((UnprocessableEntityException)context.Exception).Errors
            };

            context.Result = new UnprocessableEntityObjectResult(details);
        }

        private static void HandleUnknownException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}