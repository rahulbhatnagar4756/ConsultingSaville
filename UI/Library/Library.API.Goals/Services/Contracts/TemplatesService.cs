using Library.API.Goals.Models;
using Library.API.Goals.Models.Templates;
using Library.API.Service;
using Library.Database.DAL;
using Library.Security.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Services.Contracts
{
    public interface ITemplatesService
    {
        Task<List<TemplatesModel>?> GetAllTemplates();
        Task<List<ResultsModel>?> Save(TemplateBasicModel template);
        Task<ResultsModel?> Delete(string uuid);

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A list of UsersModel objects, or null if no users are found.</returns>
        Task<List<UsersModel>?> GetAllUsers(UserTemplatesModel userTemplates);

        /// <summary>
        /// Retrieves all users linked to a specific user template asynchronously.
        /// </summary>
        /// <param name="userTemplates">The user template to check for linked users.</param>
        /// <returns>
        /// A list of <see cref="UsersModel"/> representing all users linked to the template,
        /// or <c>null</c> if no users are linked.
        /// </returns>
        Task<List<UsersModel>?> GetAllTemplateLinkedUsers(UserTemplatesModel userTemplates);

        /// <summary>
        /// Saves or updates a user template.
        /// </summary>
        /// <param name="userTemplates">The user template data sent in the request body.</param>
        /// <returns>A list of ResultsModel objects indicating the result of the save operation.</returns>
        Task<List<ResultsModel>?> UserTemplateSave([FromBody] UserTemplatesModel userTemplates);

        /// <summary>
        /// Deletes a user template asynchronously.
        /// </summary>
        /// <param name="userTemplates">The user template to delete.</param>
        /// <returns>
        /// A list of <see cref="ResultsModel"/> representing the result of the deletion,
        /// or <c>null</c> if the deletion failed or no data was returned.
        /// </returns>
        Task<List<ResultsModel>?> UserTemplateDelete([FromBody] UserTemplatesModel userTemplates);

    }

    public class TemplatesService : ITemplatesService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;

        private readonly string _urlBase;
        private readonly string _endPointUsers = "Goals/Templates/GetAllUsers";
        private readonly string _endPointTemplates = "Goals/Templates/AllTemplates";
        private readonly string _endPointTemplatesSave = "Goals/Templates/Save";
        private readonly string _endPointTemplatesDelete = "Goals/Templates/Delete";
        private readonly string _endPointUserTemplateSave = "Goals/Templates/UserTemplateSave"; 
        private readonly string _endPointUserTemplateDelete = "Goals/Templates/UserTemplateDelete";

        private readonly string _endPointLinkedUserTemplate = "Goals/Templates/LinkedUserTemplate";


        public TemplatesService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Base:URL").Value;
        }

        public async Task<List<TemplatesModel>?> GetAllTemplates()
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            var Results = await _aPIConnect.GetAsync<List<TemplatesModel>>(Token, $"{_urlBase}{_endPointTemplates}");

            return Results;
        }

        public async Task<List<ResultsModel>?> Save(TemplateBasicModel template)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;
            var Results = await _aPIConnect.PostAsync<List<ResultsModel>, TemplateBasicModel>(Token, $"{_urlBase}{_endPointTemplatesSave}", template);
            return Results;
        }

        public async Task<ResultsModel?> Delete(string uuid)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;
          
            var Results = await _aPIConnect.PostAsync<List<ResultsModel>, dynamic>(Token, $"{_urlBase}{_endPointTemplatesDelete}", uuid);
            return Results?.FirstOrDefault()??null;

        }


        /// <summary>
        /// Retrieves all users by calling the external API.
        /// </summary>
        /// <returns>
        /// A list of <see cref="UsersModel"/> objects, or null if the token cannot be retrieved 
        /// or the API call fails.
        /// </returns>
        public async Task<List<UsersModel>?> GetAllUsers(UserTemplatesModel userTemplates)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            var Results = await _aPIConnect
           .PostAsync<List<UsersModel>, UserTemplatesModel>(Token, $"{_urlBase}{_endPointUsers}", userTemplates);

            return Results;
        }

        /// <summary>
        /// Saves or updates a user template by sending the data to the external API.
        /// </summary>
        /// <param name="userTemplates">The user template data to be saved.</param>
        /// <returns>
        /// A list of <see cref="ResultsModel"/> objects representing the result of the save operation,
        /// or null if the token cannot be retrieved.
        /// </returns>
        public async Task<List<ResultsModel>?> UserTemplateSave(UserTemplatesModel userTemplates)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            var Results = await _aPIConnect
                .PostAsync<List<ResultsModel>, UserTemplatesModel>(Token, $"{_urlBase}{_endPointUserTemplateSave}", userTemplates);

            return Results;
        }


        public async Task<List<ResultsModel>?> UserTemplateDelete(UserTemplatesModel userTemplates)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            var Results = await _aPIConnect
                .PostAsync<List<ResultsModel>, UserTemplatesModel>(Token, $"{_urlBase}{_endPointUserTemplateDelete}", userTemplates);

            return Results;
        }


        public async Task<List<UsersModel>?> GetAllTemplateLinkedUsers(UserTemplatesModel userTemplates)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            var Results = await _aPIConnect
                .PostAsync<List<UsersModel>, UserTemplatesModel>(Token, $"{_urlBase}{_endPointLinkedUserTemplate}", userTemplates);

            return Results;
        }
    }
}
