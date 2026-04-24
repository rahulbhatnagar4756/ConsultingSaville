using Library.Assess.DataAccess.Projects;
using Library.Assess.Models;
using Library.Assess.Models.Projects;
using Library.Database.DAL;
using Library.Tools.Models.Tokens;
using Library.Tools.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Assess.BusinessLogic.Projects;

internal class CandidateAutoLoginProcessor
{
    private readonly ISqlDataAccess _sql;
    private readonly IConfiguration _configuration;

    public CandidateAutoLoginProcessor(ISqlDataAccess sql, IConfiguration configuration)
    {
        _sql = sql;
        _configuration = configuration;
    }

    /// <summary>
    /// Processes auto-login using JWT token
    /// </summary>
    /// <param name="token">JWT token containing CompanyUUID, ProjectsUUID, and UsersUUID</param>
    /// <returns>Validation result with user information if successful</returns>
    public async Task<CandidateAutoLoginResult> ProcessAutoLogin(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return new CandidateAutoLoginResult
            {
                IsValid = false,
                Message = "Token is required for auto-login."
            };
        }

        try
        {
            // Step 1: Decode and validate the token
            var tokenSettings = GetTokenSettings();
            var decodedToken = token.DecodeToken(tokenSettings, validateExpiration: true);

            if (!decodedToken.IsValid)
            {
                return new CandidateAutoLoginResult
                {
                    IsValid = false,
                    Message = decodedToken.Message
                };
            }

            // Step 2: Extract required claims from token
            var companyUUID = Token.GetClaimValue(decodedToken, "CompanyUUID");
            var projectsUUID = Token.GetClaimValue(decodedToken, "ProjectsUUID");
            var usersUUID = Token.GetClaimValue(decodedToken, "UsersUUID");

            if (string.IsNullOrEmpty(companyUUID) || 
                string.IsNullOrEmpty(projectsUUID) || 
                string.IsNullOrEmpty(usersUUID))
            {
                return new CandidateAutoLoginResult
                {
                    IsValid = false,
                    Message = "Token does not contain required user information."
                };
            }

            // Step 3: Validate the token data against the database
            var validationResult = await ValidateTokenDataInDatabase(companyUUID, projectsUUID, usersUUID, token);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            CandidateRegistrationProcessor candidateRegistrationProcessor = new CandidateRegistrationProcessor(_sql, _configuration);
            token = await candidateRegistrationProcessor.GenerateLoginToken(companyUUID, usersUUID, projectsUUID);

            // Step 4: Return successful result
            return new CandidateAutoLoginResult
            {
                IsValid = true,
                Message = "Auto-login successful.",
                CompanyUUID = companyUUID,
                ProjectsUUID = projectsUUID,
                UsersUUID = usersUUID,
                Token = token
            };
        }
        catch (Exception ex)
        {
            return new CandidateAutoLoginResult
            {
                IsValid = false,
                Message = "An unexpected error occurred during auto-login validation."
            };
        }
    }

    /// <summary>
    /// Validates token data against the database
    /// </summary>
    /// <param name="companyUUID">Company UUID from token</param>
    /// <param name="projectsUUID">Project UUID from token</param>
    /// <param name="usersUUID">User UUID from token</param>
    /// <param name="token">Original token for database comparison</param>
    /// <returns>Validation result</returns>
    private async Task<CandidateAutoLoginResult> ValidateTokenDataInDatabase(
        string companyUUID, 
        string projectsUUID, 
        string usersUUID, 
        string token)
    {
        try
        {
            // Create a validation model to check against the database
            var validationModel = new TokenValidationModel
            {
                CompanyUUID = companyUUID,
                ProjectsUUID = projectsUUID,
                UsersUUID = usersUUID 
            };

            // Call the database to validate the token and associated data
            var dbValidationResult = await ProjectsUsersDataAccess.ValidateTokenData(_sql, validationModel);

            if (dbValidationResult == null)
            {
                return new CandidateAutoLoginResult
                {
                    IsValid = false,
                    Message = "Database validation failed due to an unexpected error."
                };
            }

            if (!dbValidationResult.isValid)
            {
                return new CandidateAutoLoginResult
                {
                    IsValid = false,
                    Message = dbValidationResult.Message ?? "Invalid token or user data."
                };
            }

            return new CandidateAutoLoginResult
            {
                IsValid = true,
                Message = "Token and user data validated successfully."
            };
        }
        catch (Exception ex)
        {
            return new CandidateAutoLoginResult
            {
                IsValid = false,
                Message = "Database validation failed."
            };
        }
    }

    /// <summary>
    /// Gets token settings from configuration
    /// </summary>
    /// <returns>Token settings for validation</returns>
    private TokenSettingsModel GetTokenSettings()
    {
        return new TokenSettingsModel
        {
            SecretKey = _configuration.GetSection("jwtUsersAutoLogin:Key")?.Value ?? 
                       "mY7!pQ2#vR8^sT5@wL1$zB6&nK3*eF9%jU4!xA0^cD7#hG5@qS2$uN8&bM6*oP1%",
            Issuer = _configuration.GetSection("jwtUsersAutoLogin:Issuer")?.Value ?? "Library.Assess",
            Audience = _configuration.GetSection("jwtUsersAutoLogin:Audience")?.Value ?? "Library.Assess.Users"
        };
    }
}


