using Application.Common.Interfaces;
using Application.Common.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Infrastructure.Services
{
    public class UriComposer : IUriComposer
    {
        private readonly AppClientUrlsSettings _clientUrlsSettings;

        public UriComposer(AppClientUrlsSettings appClientUrlsSettings)
        {
            _clientUrlsSettings = appClientUrlsSettings;
        }

        public string GetEmailConfirmationUrl(string email, string token)
        {
            var encodedToken = Base64UrlEncode(token);
            var callBackUrl = _clientUrlsSettings.BaseUrl + _clientUrlsSettings.EmailConfirmationCallbackUri;
            var queryParams = new Dictionary<string, string>
            {
                [nameof(email)] = email,
                [nameof(token)] = encodedToken,
            };

            var queryString = QueryString.Create(queryParams!);
            
            return callBackUrl + queryString;
        }

        public string GetPostDetailUrl(Guid postId)
        {
            return _clientUrlsSettings.BaseUrl + _clientUrlsSettings.PostDetailUri + postId;
        }

        public string Base64UrlEncode(string value)
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(value));
        }        
    }
}