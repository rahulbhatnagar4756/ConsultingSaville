using Library.Assess.BusinessLogic.Projects;
using Library.Assess.DataAccess.Projects;
using Library.Assess.Models;
using Library.Assess.Models.Projects;
using Library.Database.DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Services.Projects
{
    public interface IProjectsUsersService
    {
        Task<ResultsModel> RegisterCandidateAsync(CandidateRegistrationModel model);
        Task<CandidateAutoLoginResult> ValidateAutoLoginToken(string token);
        Task<ProjectValidationModel?> ValidateQuickLinkAsync(string quickLinkCode);
    }

    public class ProjectsUsersService : IProjectsUsersService
    {
        private readonly ISqlDataAccess _sql;
        private readonly IConfiguration _configuration;

        public ProjectsUsersService(ISqlDataAccess sql, IConfiguration configuration)
        {
            _sql = sql;
            _configuration = configuration;
        }

        public async Task<ResultsModel> RegisterCandidateAsync(CandidateRegistrationModel model)
        {
            try
            {
                var processor = new CandidateRegistrationProcessor(_sql, _configuration);
                var result = await processor.ProcessSelfRegistration(model);

                return result?.Results ?? new ResultsModel
                {
                    isValid = false,
                    Message = "Registration failed"
                };
            }
            catch (ArgumentNullException ex)
            {
                return new ResultsModel
                {
                    isValid = false,
                    Message = "Invalid registration data"
                };
            }
            catch (InvalidOperationException ex)
            {
                return new ResultsModel
                {
                    isValid = false,
                    Message = "Registration failed. Please try again."
                };
            }
        }

        public async Task<ProjectValidationModel?> ValidateQuickLinkAsync(string quickLinkCode)
        {
            if (string.IsNullOrWhiteSpace(quickLinkCode))
            {
                return new ProjectValidationModel
                {
                    isSuccessful = false,
                    Message = "Quick link code is required."
                };
            }

            try
            {
                var result = await ProjectsUsersDataAccess.ValidateQuickLink(_sql, quickLinkCode);

                return result ?? new ProjectValidationModel
                {
                    isSuccessful = false,
                    Message = "Validation failed due to an unexpected error."
                };
            }
            catch (Exception ex)
            {
                return new ProjectValidationModel
                {
                    isSuccessful = false,
                    Message = "An unexpected error occurred during validation."
                };
            }
        }

        //build a method to validate the token data against the database
        public async Task<CandidateAutoLoginResult> ValidateAutoLoginToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new CandidateAutoLoginResult
                {
                    IsValid = false,
                    Message = "Invalid parameters for token validation"
                };
            }
            try
            {
                CandidateAutoLoginProcessor candidateAutoLoginProcessor = new CandidateAutoLoginProcessor(_sql, _configuration);
                var ValidationResult = await candidateAutoLoginProcessor.ProcessAutoLogin(token);
                if (ValidationResult == null)
                {
                    return new CandidateAutoLoginResult
                    {
                        IsValid = false,
                        Message = "Validation failed due to an unexpected error."
                    };
                }
                if (!ValidationResult.IsValid)
                {
                    return new CandidateAutoLoginResult
                    {
                        IsValid = false,
                        Message = ValidationResult.Message ?? "Invalid token or user data."
                    };
                }

                return ValidationResult;
            }
            catch (Exception ex)
            {
                return new CandidateAutoLoginResult
                {
                    IsValid = false,
                    Message = "An unexpected error occurred during token validation."
                };
            }
        }

    }
}
