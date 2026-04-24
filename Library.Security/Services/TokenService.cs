using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace Library.Security.Services
{
    public interface ITokenService
    {
        Task<string?> GetToken();
    }

    public class TokenService : ITokenService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _cookieName = "Security_API";
        private string _cookieSubName = "Access";

        public TokenService(IJSRuntime jsRuntime, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _jsRuntime = jsRuntime;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the JWT token from the cookies, you
        /// can call this only when the page is loaded
        /// </summary>
        /// <returns></returns>
        public async Task<string?> GetToken()
        {
            // Implement logic to retrieve JWT token from cookie(Security_API) using JavaScript interop
            //return await _jsRuntime.InvokeAsync<string>("getCookie", "Security_API");
            var httpContext = _httpContextAccessor.HttpContext;
            string? Cookie = "";

            if (httpContext == null)
            {
                try
                {
                    Cookie = await GetTokenFromCookieJsAsync();
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                    try
                    {
                        Cookie = httpContext.Request.Cookies[_cookieName] ?? await GetTokenFromCookieJsAsync();
                    }
                    catch
                    {
                        return null;
                    }
            }

            if (string.IsNullOrWhiteSpace(Cookie)) return null;
            return await ExtractKeyValue(Cookie, _cookieSubName);
        }

        /// <summary>
        /// This is the javascript function that is being called, please create a javascript file and add this function to it and link it the project master page
        ///    function getCookie(name)
        ///    {
        ///        let matches = document.cookie.match(new RegExp(
        ///            "(?:^|; )" + name.replace(/ ([\.$?*|{ }\(\)\[\]\\\/\+^])/ g, '\\$1') +"=([^;]*)"
        ///        ));
        ///        return matches ? decodeURIComponent(matches[1]) : null;
        ///    }
        /// </summary>
        /// <returns></returns>
        private async Task<string?> GetTokenFromCookieJsAsync() =>
        await _jsRuntime.InvokeAsync<string>("getCookie", _cookieName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        private async Task<string> ExtractKeyValue(string cookie, string Key)
        {
            var keyValuePairs = cookie.Split('&');
            foreach (var pair in keyValuePairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue[0] == Key)
                {
                    return keyValue[1];
                }
            }
            return null;
        }

    }
}
