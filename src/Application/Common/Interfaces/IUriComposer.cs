namespace Application.Common.Interfaces
{
    public interface IUriComposer
    {
        string GetEmailConfirmationUrl(string email, string token);

        string GetPostDetailUrl(Guid postId);

        string Base64UrlEncode(string value);
    }
}