using Dapper;
using Library.Assess.Mapper.Projects;
using Library.Assess.Models;
using Library.Assess.Models.Projects;
using Library.Assess.Models.Tests; 
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.DataAccess.Projects;

internal class ProjectsUsersDataAccess
{
    public static async Task<TestAssignmentResultModel?> PublicsSelfRegisterCandidate(ISqlDataAccess sql, CandidateRegistrationModel model)
    {
        var parameters = model.ToDynamicParameters(); 
        var result = await sql.ExecuteWithOutputsAsync<TestAssignmentModel>("[Project].[spProjectsUsers_Register]", parameters);
        var Data = new TestAssignmentResultModel
        {
            TestAssignments = result?.ToList() ?? null,
            Results = new ResultsModel() { UUID = parameters.Get<string>("@UsersUUIDCandidate")
                                         , isValid = parameters.Get<bool>("@isSuccess")
                                         , Message = parameters.Get<string>("@Message") }
        };

        return Data;
    }

    public static async Task<ProjectValidationModel?> ValidateQuickLink(ISqlDataAccess sql, string quickLinkCode)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@QuickLinkCode", quickLinkCode);
        parameters.Add("@ProjectsUUID", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        parameters.Add("@ProjectName", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
        parameters.Add("@CompanyUUID", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        parameters.Add("@isSuccessful", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        parameters.Add("@Message", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

        var result = await sql.ExecuteWithOutputsAsync("[Project].[spProjectQuickLinks_Validate]", parameters);

        if (result == null) return null;

        return new ProjectValidationModel
        {
            ProjectsUUID = result.Get<string>("@ProjectsUUID"),
            ProjectName = result.Get<string>("@ProjectName"),
            CompanyUUID = result.Get<string>("@CompanyUUID"),
            isSuccessful = result.Get<bool>("@isSuccessful"),
            Message = result.Get<string>("@Message")
        };
    }

    /// <summary>
    /// Saves a token for a project user assignment
    /// </summary>
    /// <param name="sql">SQL data access interface</param>
    /// <param name="model">Save token model containing the token and user/project information</param>
    /// <returns>Result of the save operation</returns>
    public static async Task<SaveTokenResultModel?> SaveToken(ISqlDataAccess sql, SaveTokenModel model)
    {
        try
        {
            var parameters = model.ToDynamicParameters();
            var result = await sql.ExecuteWithOutputsAsync("[Project].[spProjectsUsers_Save_Token]", parameters);

            if (result == null) return null;

            return new SaveTokenResultModel
            {
                IsSuccessful = result.Get<bool>("@isSuccessful"),
                Message = result.Get<string>("@Message") ?? string.Empty
            };
        }
        catch (Exception)
        {
            return new SaveTokenResultModel
            {
                IsSuccessful = false,
                Message = "An unexpected error occurred while saving the token."
            };
        }
    }

    /// <summary>
    /// Validates token data against the database
    /// </summary>
    /// <param name="sql">SQL data access interface</param>
    /// <param name="model">Token validation model</param>
    /// <returns>Validation result</returns>
    public static async Task<ResultsModel?> ValidateTokenData(ISqlDataAccess sql, TokenValidationModel model)
    {
        try
        {
            var parameters = model.ToDynamicParameters();
            var result = await sql.ExecuteWithOutputsAsync("[Project].[spProjectsUsers_Validate_Token]", parameters);

            if (result == null) return null;

            return new ResultsModel
            {
                isValid = result.Get<bool>("@isSuccessful"),
                Message = result.Get<string>("@Message") ?? string.Empty
            };
        }
        catch (Exception)
        {
            return new ResultsModel
            {
                isValid = false,
                Message = "An unexpected error occurred during token validation."
            };
        }
    }
}
