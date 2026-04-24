using Dapper;
using Library.Base.Models;
using Library.Base.Models.Users;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.DataAccess.Users;

internal static class UsersDataAccess
{
    public static async Task<IEnumerable<UserBasicModel>> GetUserByUUID(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedin, string usersUUID) =>
        await sql.LoadDataAsync<UserBasicModel, dynamic>("[base].[spUsersByUUID]", new { CompanyUUID = companyUUID, UsersUUIDLoggedin = usersUUIDLoggedin, UsersUUID = usersUUID });

    public static async Task<IEnumerable<UserBasicInformationModel>> GetUserBasicInformationByUUID(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedin, string usersUUID) =>
        await sql.LoadDataAsync<UserBasicInformationModel, dynamic>("[base].[spUsers_Basic_ByUUID]", new { CompanyUUID = companyUUID, UsersUUIDLoggedin = usersUUIDLoggedin, UsersUUID = usersUUID });

    public static async Task<UserBasicInformationModel> GetUserBasic(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedin, string usersUUID)
    {
        var result = await sql.LoadDataAsync<UserBasicInformationModel, dynamic>("[base].[spUsers_Basic]", new { CompanyUUID = companyUUID, UsersUUIDLoggedin = usersUUIDLoggedin, UsersUUID = usersUUID });
        return result?.FirstOrDefault() ?? new UserBasicInformationModel();
    }

    /// <summary>
    /// Save user basic information
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="companyUUID"></param>
    /// <param name="usersUUIDLoggedIn"></param>
    /// <param name="userInformation"></param>
    /// <returns></returns>
    public static async Task<ResultsModel?> SaveBasic(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedIn, UserBasicInformationModel userInformation)
    {
        var parameters = new DynamicParameters();

        // input params
        parameters.Add("@CompanyUUID", companyUUID, DbType.String, ParameterDirection.Input, 200);
        parameters.Add("@UsersUUIDLoggedIn", usersUUIDLoggedIn, DbType.String, ParameterDirection.Input, 200);
        parameters.Add("@UUID", userInformation?.UUID, DbType.String, ParameterDirection.InputOutput, 200);
        parameters.Add("@FirstName", userInformation?.FirstName, DbType.String, ParameterDirection.Input, 200);
        parameters.Add("@LastName", userInformation?.LastName, DbType.String, ParameterDirection.Input, 200);
        parameters.Add("@IDNumber", userInformation?.IDNumber, DbType.String, ParameterDirection.Input, 200);
        parameters.Add("@Email", userInformation?.Email, DbType.String, ParameterDirection.Input, 200);
        parameters.Add("@Mobile", userInformation?.Mobile, DbType.String, ParameterDirection.Input, 200);
        parameters.Add("@Gender", userInformation?.Gender, DbType.String, ParameterDirection.Input, 200);
        parameters.Add("@Ethnicity", userInformation?.Ethnicity, DbType.String, ParameterDirection.Input, 200);
        parameters.Add("@DateOfBirth", userInformation?.DateOfBirth, DbType.DateTime, ParameterDirection.Input);

        // output params
        parameters.Add("@isSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

        // execute stored procedure and retrieve output parameters
        var output = await sql.ExecuteWithOutputsAsync("[base].[spUsers_Basic_Save]", parameters);
        if (output == null) return null;

        return new ResultsModel
        {
            UUID = output.Get<string>("@UUID") ?? output.Get<string>("@UUID"),
            isValid = output.Get<bool?>("@isSuccess"),
            Message = output.Get<string>("@Message")
        };
    }

    /// <summary>
    /// Save user image
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="companyUUID"></param>
    /// <param name="usersUUIDLoggedIn"></param>
    /// <param name="userUUID"></param>
    /// <param name="imageBytes"></param>
    /// <returns></returns>
    public static async Task<UserImageSaveModel?> SaveImage(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedIn, string userUUID, byte[] imageBytes)
    {
        DynamicParameters parameters = new();
        parameters.Add("@CompanyUUID", companyUUID);
        parameters.Add("@UsersUUIDLoggedIn", usersUUIDLoggedIn);
        parameters.Add("@UUID", userUUID);
        parameters.Add("@ImageFileLocation", null, System.Data.DbType.String, System.Data.ParameterDirection.Output );
        parameters.Add("@isSuccess", null, System.Data.DbType.Boolean, System.Data.ParameterDirection.Output);
        parameters.Add("@Message", null,  System.Data.DbType.String, System.Data.ParameterDirection.Output );

        var result = await sql.ExecuteWithOutputsAsync("[base].[spUsers_Basic_Image_Save]", parameters);
        if (result == null) return null;

        return new UserImageSaveModel
        {
            CompanyUUID = companyUUID,
            UsersUUIDLoggedIn = usersUUIDLoggedIn,
            UUID = userUUID,
            ImageBytes = imageBytes,
            ImageFileLocation = result.Get<string>("@ImageFileLocation"),
            IsSuccess = result.Get<bool>("@isSuccess"),
            Message = result.Get<string>("@Message")
        };
    }

    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="companyUUID"></param>
    /// <param name="usersUUIDLoggedIn"></param>
    /// <param name="userUUID"></param>
    /// <returns></returns>
    public static async Task<ResultsModel?> Delete(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedIn, string userUUID)
    {
        var parameters = new
        {
            CompanyUUID = companyUUID,
            UsersUUIDLoggedIn = usersUUIDLoggedIn,
            UUID = userUUID
        };

        var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[base].[spUsers_Basic_Delete]", parameters);
        if (result == null) return null;

        return result?.FirstOrDefault()??null;
    }

    /// <summary>
    /// get the Ethnicity lookup list
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="companyUUID"></param>
    /// <param name="usersUUIDLoggedin"></param>
    /// <returns>ID, Name, and Orderval</returns>
    public static async Task<IEnumerable<LookupBasicModel>?> GetEthnicity(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedin) =>
        await sql.LoadDataAsync<LookupBasicModel, dynamic>("[base].[spLookup_Ethnicity]", new { CompanyUUID = companyUUID, UsersUUIDLoggedin = usersUUIDLoggedin });

    /// <summary>
    /// Get the Gender lookup list
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="companyUUID"></param>
    /// <param name="usersUUIDLoggedin"></param>
    /// <returns>ID, Name, and Orderval</returns>
    public static async Task<IEnumerable<LookupBasicModel>?> GetGender(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedin) =>
        await sql.LoadDataAsync<LookupBasicModel, dynamic>("[base].[spLookup_Gender]", new { CompanyUUID = companyUUID, UsersUUIDLoggedin = usersUUIDLoggedin });

    public static async Task<string?> GetUsersUUIDByIDNumber(ISqlDataAccess sql, string idNumber) =>
        await sql.LoadFirstDataAsync<string, dynamic>("[base].[spUsers_UUID_ByIDNumber]", new { IDNumber = idNumber });

    public static async Task<IEnumerable<NamesModel>?> GetUsersNamesSearch(ISqlDataAccess sql, string companyUUID, string UsersUUIDLoggedIn, string search, bool isIncludeTeam = false) =>
        await sql.LoadDataAsync<NamesModel, dynamic>("[base].[spUsers_Names_Search]", new { CompanyUUID = companyUUID, UsersUUIDLoggedIn = UsersUUIDLoggedIn, Search = search, isIncludeTeam = isIncludeTeam });
}
