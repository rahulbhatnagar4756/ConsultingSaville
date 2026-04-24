using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library.Security.AuthenticationStateProviders
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string? _cachedCookie;
        private string _cookieName = "Security_API";
        private string _cookieSubName = "Access";

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, IConfiguration configuration, IHttpContextAccessor HttpContextAccessor)
        {
            _jsRuntime = jsRuntime;
            _configuration = configuration;
            _httpContextAccessor = HttpContextAccessor;
        }

        private async Task<AuthenticationState> GetAuthenticationStateInfoAsync()
        {
            var token = await GetTokenFromCookieAsync();
            if (token == null)
            {
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSigningKey(),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;

            ClaimsPrincipal? principal = null;
            try
            {
                principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

            }
            catch (SecurityTokenExpiredException)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;

            }

            if (principal == null)
            {
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
            }

            var claims = principal.Claims.ToList();
            string userId = claims.FirstOrDefault(x => x.Type == "Usersid")?.Value ?? "";
            string companyId = claims.FirstOrDefault(x => x.Type == "Companyid")?.Value ?? "";
            var roles = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

            var initialClaims = new List<Claim>
                                {
                                    new Claim("Usersid" , userId),
                                    new Claim("Companyid", companyId)
                                };

            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            var combinedClaims = initialClaims.Concat(roleClaims);

            var user = new ClaimsPrincipal(new ClaimsIdentity(combinedClaims, "CustomAuthentication"));
            return await Task.FromResult(new AuthenticationState(user));
        }

        /// <summary>
        /// Get the Token from the Browser Cookies
        /// </summary>
        /// <returns></returns>
        private async Task<string?> GetTokenFromCookieAsync()
        {
            // Implement logic to retrieve JWT token from cookie(Security_API) using JavaScript interop
            //return await _jsRuntime.InvokeAsync<string>("getCookie", "Security_API");
            var httpContext = _httpContextAccessor.HttpContext;
            string? Cookie = "";

            if (httpContext == null)
            {
                if (!string.IsNullOrWhiteSpace(_cachedCookie)) Cookie = _cachedCookie;
                else
                {
                    try { 
                        Cookie = await GetTokenFromCookieJsAsync();
                    }
                    catch { 
                        return null;
                    }
                }
            }
            else
            {
                Cookie = httpContext.Request.Cookies[_cookieName];
            }
            
            if (string.IsNullOrWhiteSpace(Cookie)) return null;
            return await ExtractKeyValue(Cookie, _cookieSubName);
        }

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

        private SecurityKey GetSigningKey()
        {
            // Implement logic to retrieve the signing key for the JWT token
            string? Key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(Key)) return null;
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync() =>
            await GetAuthenticationStateInfoAsync();


        public async Task<Models.LoggedInUserInformation> LoggedInUserInformation()
        {
            return null;
        }

    }
}
