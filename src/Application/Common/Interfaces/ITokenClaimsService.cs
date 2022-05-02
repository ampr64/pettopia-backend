namespace Application.Common.Interfaces
{
    public interface ITokenClaimsService
    {
        Task<string?> GetTokenAsync(string email);
    }
}