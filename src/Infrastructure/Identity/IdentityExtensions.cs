using Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public static class IdentityExtensions
    {
        public static Result<T?> ToResultObject<T>(this IdentityResult result, T? data)
        {
            return result.Succeeded
                ? Result<T?>.Success(data)
                : Result<T?>.Failure(result.Errors.Select(e => e.Description));
        }
    }
}