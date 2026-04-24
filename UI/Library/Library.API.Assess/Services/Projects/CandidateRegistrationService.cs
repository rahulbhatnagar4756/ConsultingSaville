using Library.API.Assess.Models;
using Library.API.Assess.Models.Projects;
using Library.API.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Library.API.Assess.Services.Projects
{
    public interface ICandidateRegistrationService
    {
        Task<ResultsModel> RegisterCandidateAsync(CandidateRegistrationModel model);
        Task<ProjectValidationModel> ValidateQuickLinkAsync(string quickLinkCode);
    }

    public class CandidateRegistrationService : ICandidateRegistrationService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly string _urlBase;
        private readonly string _endPointRegister = "Assess/Projects/CandidateRegistration/Register";
        private readonly string _endPointValidateQuickLink = "Assess/Projects/CandidateRegistration/ValidateQuickLink";

        public CandidateRegistrationService(IConfiguration config, IAPIConnectService aPIConnect)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _urlBase = _config.GetSection("API:Base:URL").Value;
        }

        public async Task<ResultsModel> RegisterCandidateAsync(CandidateRegistrationModel model)
        {
            if (model == null)
                return new ResultsModel { isValid = false, Message = "Invalid registration data." };

            try
            {
                var response = await _aPIConnect.PostAsync<ResultsModel, CandidateRegistrationModel>(
                    $"{_urlBase}{_endPointRegister}", model);

                return response ?? new ResultsModel
                {
                    isValid = false,
                    Message = "Registration failed. Please try again."
                };
            }
            catch (Exception ex)
            {
                return new ResultsModel
                {
                    isValid = false,
                    Message = "An unexpected error occurred. Please try again."
                };
            }
        }

        public async Task<ProjectValidationModel> ValidateQuickLinkAsync(string quickLinkCode)
        {
            if (string.IsNullOrWhiteSpace(quickLinkCode))
                return new ProjectValidationModel
                {
                    isSuccessful = false,
                    Message = "Quick link code is required."
                };

            try
            {
                var response = await _aPIConnect.GetAsync<ProjectValidationModel>(
                    $"{_urlBase}{_endPointValidateQuickLink}/{quickLinkCode}");

                return response ?? new ProjectValidationModel
                {
                    isSuccessful = false,
                    Message = "Validation failed. Please try again."
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
    }
}