using System.Security.Claims;

namespace Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        ClaimsPrincipal? Principal { get; }
    }
}