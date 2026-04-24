using Library.Assess.BusinessLogic.Projects.TestAssignmentProcessors;
using Library.Assess.DataAccess.Projects;
using Library.Assess.Enumerators;
using Library.Assess.Models;
using Library.Assess.Models.Projects;
using Library.Assess.Models.Tests;
using Library.Database.DAL;
using Library.Tools.Models.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Library.Assess.BusinessLogic.Projects
{
    internal class CandidateRegistrationProcessor
    {
        private readonly ISqlDataAccess _sql;
        private readonly IConfiguration _configuration;

        public CandidateRegistrationProcessor(ISqlDataAccess sql, IConfiguration configuration)
        {
            _sql = sql;
            _configuration = configuration;
        }

        public async Task<TestAssignmentResultModel?> ProcessSelfRegistration(CandidateRegistrationModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Candidate registration model cannot be null");

            try
            {
                var result = await ProjectsUsersDataAccess.PublicsSelfRegisterCandidate(_sql, model);

                if (result?.TestAssignments?.Any() == true)
                {
                    await ProcessTestAssignments(result.TestAssignments);
                }

                if (result.Results.isValid)
                {
                    //generate the token for the user, company, project UUIDs
                    var Token = await GenerateToken(model.CompanyUUID,
                                                     model.ProjectsUUID,
                                                     result.Results.UUID); // Default to 30 days


                    await ProjectsUsersDataAccess.SaveToken(_sql, new SaveTokenModel
                    {
                        CompanyUUID = model.CompanyUUID,
                        UsersUUIDLoggedIn = result.Results.UUID,
                        ProjectsUUID = model.ProjectsUUID,
                        UsersUUID = result.Results.UUID,
                        Token = Token
                    });

                }

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                //throw new InvalidOperationException("Failed to process candidate registration", ex);
                return default;
            }
        }

        private async Task<string> GenerateToken(string companyUUID, string projectsUUID, string usersUUID, int? ExpirationMinutes = null)
        {
            if (string.IsNullOrEmpty(companyUUID)  ||
                string.IsNullOrEmpty(projectsUUID) || 
                string.IsNullOrEmpty(usersUUID))
            {
                throw new ArgumentException("Invalid parameters for token generation");
            }
             
            List<TokenClaims> Claims = new List<TokenClaims>
            {
                new TokenClaims { Name = "CompanyUUID", Value = companyUUID },
                new TokenClaims { Name = "ProjectsUUID", Value = projectsUUID },
                new TokenClaims { Name = "UsersUUID", Value = usersUUID }
            };

            if (ExpirationMinutes is null)
            {
                if (!int.TryParse(_configuration.GetSection("jwtUsersAutoLogin:ExpireIn_Refresh").Value, out var minutes))
                    ExpirationMinutes = 60 * 24 * 30;
                else
                    ExpirationMinutes = minutes;
                // Default to 30 days if not specified
            }

            TokenSettingsModel TokenSettings = new TokenSettingsModel
            {
                SecretKey = _configuration.GetSection("jwtUsersAutoLogin:Key")?.Value ?? "mY7!pQ2#vR8^sT5@wL1$zB6&nK3*eF9%jU4!xA0^cD7#hG5@qS2$uN8&bM6*oP1%",
                Issuer = _configuration.GetSection("jwtUsersAutoLogin:Issuer")?.Value ?? "Library.Assess",
                Audience = _configuration.GetSection("jwtUsersAutoLogin:Audience")?.Value ?? "Library.Assess.Users",
                ExpirationMinutes = ExpirationMinutes.Value // Set token expiration time
            };

            return Library.Tools.Tokens.Token.GenerateToken(TokenSettings, Claims);
        }

        public async Task<string> GenerateLoginToken(string companyUUID, string usersUUID, string projectsUUID, int? ExpirationMinutes = (6 * 60))
        {
            if (string.IsNullOrEmpty(companyUUID) ||
                string.IsNullOrEmpty(usersUUID))
            {
                throw new ArgumentException("Invalid parameters for token generation");
            }

            List<TokenClaims> Claims = new List<TokenClaims>
            {
                new TokenClaims { Name = "CompanyUUID", Value = companyUUID },
                new TokenClaims { Name = "UsersUUID", Value = usersUUID },
                new TokenClaims { Name = "ProjectsUUID", Value = projectsUUID },
                new TokenClaims { Name = ClaimTypes.Role, Value = "Assessments" }
            };

            //save a role of assessment

            if (ExpirationMinutes is null)
            {
                if (!int.TryParse(_configuration.GetSection("jwt:ExpireIn_Refresh").Value, out var minutes))
                    ExpirationMinutes = 60 * 6;
                else
                    ExpirationMinutes = minutes;
                // Default to 30 days if not specified
            }

            TokenSettingsModel TokenSettings = new TokenSettingsModel
            {
                SecretKey = _configuration.GetSection("jwt:Key")?.Value ?? "6v9y$B&E)H@McQfTjWnZr4t7w!z%C*F-JaNdRgUkXp2s5v8x/A?D(G+KbPeShVmY",
                Issuer = _configuration.GetSection("jwt:Issuer")?.Value ?? "https://localhost:7118/",
                Audience = _configuration.GetSection("jwt:Audience")?.Value ?? "https://localhost:7118/",
                ExpirationMinutes = ExpirationMinutes.Value // Set token expiration time
            };

            return Library.Tools.Tokens.Token.GenerateToken(TokenSettings, Claims);
        }

        private async Task ProcessTestAssignments(List<TestAssignmentModel> assignments)
        {
            var processingTasks = assignments.Select(ProcessTestAssignment);
            await Task.WhenAll(processingTasks);
        }

        private async Task ProcessTestAssignment(TestAssignmentModel assignment)
        {
            var processor = TestAssignmentProcessorFactory.CreateProcessor(assignment.IntegrationUUID);
            await processor.ProcessAsync(assignment);
        }

    }
}