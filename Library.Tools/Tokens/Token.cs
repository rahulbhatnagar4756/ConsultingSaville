using Library.Tools.Models.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library.Tools.Tokens
{
    /// <summary>
    /// Static utility class for JWT token generation and decoding
    /// </summary>
    public static class Token
    {
        /// <summary>
        /// Generates a JWT token with the specified settings and claims
        /// </summary>
        /// <param name="settings">Token settings including secret key, issuer, audience, and expiration</param>
        /// <param name="claims">Collection of claims to include in the token</param>
        /// <param name="customExpiration">Optional custom expiration time (overrides settings)</param>
        /// <returns>Generated JWT token string</returns>
        /// <exception cref="ArgumentNullException">Thrown when settings or claims are null</exception>
        /// <exception cref="ArgumentException">Thrown when SecretKey is null or empty</exception>
        public static string GenerateToken(TokenSettingsModel settings, List<TokenClaims> claims, DateTime? customExpiration = null)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrWhiteSpace(settings.SecretKey))
                throw new ArgumentException("SecretKey cannot be null or empty", nameof(settings));

            if (claims == null)
                throw new ArgumentNullException(nameof(claims));

            // Create security key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Build claims list
            var tokenClaims = new List<Claim>();

            // Add custom claims
            foreach (var claim in claims.Where(c => !string.IsNullOrWhiteSpace(c.Name)))
            {
                tokenClaims.Add(new Claim(claim.Name, claim.Value ?? string.Empty));
            }

            // Add standard claims if not already present
            if (!tokenClaims.Any(c => c.Type == JwtRegisteredClaimNames.Jti))
            {
                tokenClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            }

            if (!tokenClaims.Any(c => c.Type == JwtRegisteredClaimNames.Iat))
            {
                tokenClaims.Add(new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64));
            }

            // Calculate expiration
            var expirationTime = customExpiration ?? DateTime.UtcNow.AddMinutes(settings.ExpirationMinutes);

            // Create token
            var token = new JwtSecurityToken(
                issuer: settings.Issuer,
                audience: settings.Audience,
                claims: tokenClaims,
                expires: expirationTime,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generates a JWT token with additional options for flexibility
        /// </summary>
        /// <param name="settings">Token settings</param>
        /// <param name="claims">Collection of claims</param>
        /// <param name="subject">Subject (SUB claim)</param>
        /// <param name="roles">Additional roles to include</param>
        /// <param name="customExpiration">Custom expiration time</param>
        /// <param name="notBefore">Not before time (NBF claim)</param>
        /// <returns>Generated JWT token string</returns>
        public static string GenerateToken(
            TokenSettingsModel settings,
            IEnumerable<TokenClaims> claims,
            string? subject = null,
            IEnumerable<string>? roles = null,
            DateTime? customExpiration = null,
            DateTime? notBefore = null)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrWhiteSpace(settings.SecretKey))
                throw new ArgumentException("SecretKey cannot be null or empty", nameof(settings));

            if (claims == null)
                throw new ArgumentNullException(nameof(claims));

            // Create security key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Build claims list
            var tokenClaims = new List<Claim>();

            // Add custom claims
            foreach (var claim in claims.Where(c => !string.IsNullOrWhiteSpace(c.Name)))
            {
                tokenClaims.Add(new Claim(claim.Name, claim.Value ?? string.Empty));
            }

            // Add standard claims
            tokenClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            tokenClaims.Add(new Claim(JwtRegisteredClaimNames.Iat,
                new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64));

            // Add subject if provided
            if (!string.IsNullOrWhiteSpace(subject))
            {
                tokenClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, subject));
            }

            // Add roles if provided
            if (roles?.Any() == true)
            {
                foreach (var role in roles)
                {
                    tokenClaims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            // Calculate expiration
            var expirationTime = customExpiration ?? DateTime.UtcNow.AddMinutes(settings.ExpirationMinutes);

            // Create token
            var token = new JwtSecurityToken(
                issuer: settings.Issuer,
                audience: settings.Audience,
                claims: tokenClaims,
                expires: expirationTime,
                signingCredentials: credentials,
                notBefore: notBefore
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Decodes and validates a JWT token, returning claims if valid
        /// </summary>
        /// <param name="token">JWT token to decode and validate</param>
        /// <param name="settings">Token settings for validation</param>
        /// <param name="validateExpiration">Whether to validate token expiration (default: true)</param>
        /// <returns>TokenClaimsResult containing validation status and claims</returns>
        public static TokenClaimsResult DecodeToken(this string token, TokenSettingsModel settings, bool validateExpiration = true)
        {
            var result = new TokenClaimsResult();

            if (string.IsNullOrWhiteSpace(token))
            {
                result.IsValid = false;
                result.Message = "Token cannot be null or empty.";
                return result;
            }

            if (settings == null)
            {
                result.IsValid = false;
                result.Message = "Token settings cannot be null.";
                return result;
            }

            if (string.IsNullOrWhiteSpace(settings.SecretKey))
            {
                result.IsValid = false;
                result.Message = "Secret key cannot be null or empty.";
                return result;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                // First, try to read the token without validation to get claims
                if (!tokenHandler.CanReadToken(token))
                {
                    result.IsValid = false;
                    result.Message = "Invalid token format.";
                    return result;
                }

                // Read token without validation to extract claims
                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Convert JWT claims to TokenClaims
                result.Claims = jwtToken.Claims
                    .Select(c => new TokenClaims { Name = c.Type, Value = c.Value })
                    .ToList();

                // If validation is required, validate the token
                if (validateExpiration || !string.IsNullOrWhiteSpace(settings.Issuer) || !string.IsNullOrWhiteSpace(settings.Audience))
                {
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey)),
                        ValidateIssuer = !string.IsNullOrWhiteSpace(settings.Issuer),
                        ValidIssuer = settings.Issuer,
                        ValidateAudience = !string.IsNullOrWhiteSpace(settings.Audience),
                        ValidAudience = settings.Audience,
                        ValidateLifetime = validateExpiration,
                        ClockSkew = TimeSpan.Zero
                    };

                    var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                    if (principal != null)
                    {
                        result.IsValid = true;
                        result.Message = "Token is valid.";
                    }
                    else
                    {
                        result.IsValid = false;
                        result.Message = "Token validation failed.";
                    }
                }
                else
                {
                    // If no validation required, just check if we can read the token
                    result.IsValid = true;
                    result.Message = "Token decoded successfully (validation skipped).";
                }
            }
            catch (SecurityTokenExpiredException)
            {
                result.IsValid = false;
                result.Message = "Token has expired.";
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                result.IsValid = false;
                result.Message = "Invalid token signature.";
            }
            catch (SecurityTokenInvalidIssuerException)
            {
                result.IsValid = false;
                result.Message = "Invalid token issuer.";
            }
            catch (SecurityTokenInvalidAudienceException)
            {
                result.IsValid = false;
                result.Message = "Invalid token audience.";
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Message = $"Token validation error: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// Decodes a JWT token without validation (useful for expired tokens or when you only need claims)
        /// </summary>
        /// <param name="token">JWT token to decode</param>
        /// <returns>TokenClaimsResult containing claims (validation always skipped)</returns>
        public static TokenClaimsResult DecodeToken(this string token)
        {
            var result = new TokenClaimsResult();

            if (string.IsNullOrWhiteSpace(token))
            {
                result.IsValid = false;
                result.Message = "Token cannot be null or empty.";
                return result;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                if (!tokenHandler.CanReadToken(token))
                {
                    result.IsValid = false;
                    result.Message = "Invalid token format.";
                    return result;
                }

                var jwtToken = tokenHandler.ReadJwtToken(token);

                result.Claims = jwtToken.Claims
                    .Select(c => new TokenClaims { Name = c.Type, Value = c.Value })
                    .ToList();

                result.IsValid = true;
                result.Message = "Token decoded successfully (no validation performed).";
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Message = $"Token decoding error: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// Gets a specific claim value from a decoded token result
        /// </summary>
        /// <param name="claimsResult">Token claims result</param>
        /// <param name="claimName">Name of the claim to retrieve</param>
        /// <returns>Claim value if found, null otherwise</returns>
        public static string? GetClaimValue(TokenClaimsResult claimsResult, string claimName)
        {
            return claimsResult?.Claims?.FirstOrDefault(c => c.Name == claimName)?.Value;
        }

        /// <summary>
        /// Checks if a token has expired based on the 'exp' claim
        /// </summary>
        /// <param name="claimsResult">Token claims result</param>
        /// <returns>True if expired, false if still valid, null if exp claim not found</returns>
        public static bool? IsTokenExpired(TokenClaimsResult claimsResult)
        {
            var expClaim = GetClaimValue(claimsResult, "exp");
            if (string.IsNullOrWhiteSpace(expClaim) || !long.TryParse(expClaim, out var exp))
                return null;

            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
            return DateTime.UtcNow > expirationTime;
        }
    }
}