using Library.Base.DataAccess;
using Library.Base.DataAccess.Users;
using Library.Base.Models;
using Library.Base.Models.Users;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Services;

public interface IUsersService
{
    Task<List<LookupBasicModel>?> GetEthnicity(string companyUUID, string usersUUIDLoggedin);
    Task<List<LookupBasicModel>?> GetGender(string companyUUID, string usersUUIDLoggedin);
    Task<UserBasicInformationModel?> GetUserBasicInformationByUUID(string companyUUID, string usersUUIDLoggedin, string usersUUID);
    Task<UserBasicInformationModel?> GetUserBasic(string companyUUID, string usersUUIDLoggedin, string usersUUID);
    Task<UserBasicModel> GetUserByUUID(string companyUUID, string usersUUIDLoggedin, string usersUUID);
    Task<List<NamesModel>?> GetUsersNamesSearch(string companyUUID, string UsersUUIDLoggedIn, string search, bool isIncludeTeam = false);
    Task<ResultsModel> GetUsersUUIDByIDNumber(string idNumber);
    Task<ResultsModel> SaveUserBasic(string companyUUID, string usersUUIDLoggedIn, UserBasicInformationModel userInformation);
    Task<ResultsModel> SaveImage(string companyUUID, string usersUUIDLoggedIn, UserImageModel image);
    Task<ResultsModel> Delete(string companyUUID, string usersUUIDLoggedIn, string userUUID);
    Task<ResultsModel> SetUserBasicInformationByUUID(string companyUUID, string usersUUIDLoggedIn, UserBasicInformationModel userInformation);
}

public class UsersService : IUsersService
{
    private readonly ISqlDataAccess _sql;
    private readonly IUserRolesService _userRoles;

    public UsersService(ISqlDataAccess sql, IUserRolesService userRoles)
    {
        _sql = sql;
        _userRoles = userRoles;
    }

    public async Task<UserBasicModel> GetUserByUUID(string companyUUID, string usersUUIDLoggedin, string usersUUID)
    {
        IEnumerable<UserBasicModel> datas = await UsersDataAccess.GetUserByUUID(_sql, companyUUID, usersUUIDLoggedin, usersUUID);
        if (datas == null || datas.Count() == 0) return null;
        UserBasicModel data = datas.FirstOrDefault();
        data.Roles = await _userRoles.GetRolesByUsersUUID(companyUUID, usersUUIDLoggedin, usersUUID);

        return data;
    }

    public async Task<UserBasicInformationModel?> GetUserBasicInformationByUUID(string companyUUID, string usersUUIDLoggedin, string usersUUID)
    {
        IEnumerable<UserBasicInformationModel> datas = await UsersDataAccess.GetUserBasicInformationByUUID(_sql, companyUUID, usersUUIDLoggedin, usersUUID);
        if (datas == null || datas.Count() == 0) return null;
        UserBasicInformationModel data = datas.FirstOrDefault();
        return data;
    }

    public async Task<UserBasicInformationModel?> GetUserBasic(string companyUUID, string usersUUIDLoggedin, string usersUUID)
    {
        IEnumerable<UserBasicInformationModel> datas = await UsersDataAccess.GetUserBasicInformationByUUID(_sql, companyUUID, usersUUIDLoggedin, usersUUID);
        if (datas == null || datas.Count() == 0) return null;
        UserBasicInformationModel data = datas.FirstOrDefault();
        return data;
    }

    public async Task<ResultsModel> SetUserBasicInformationByUUID(string companyUUID, string usersUUIDLoggedIn, UserBasicInformationModel userInformation)
    {
        var result = await UsersDataAccess.SaveBasic(_sql, companyUUID, usersUUIDLoggedIn, userInformation);
        return result ?? new ResultsModel { isValid = false, Message = "Failed to save user information" };
    }

    public async Task<ResultsModel> SaveUserBasic(string companyUUID, string usersUUIDLoggedIn, UserBasicInformationModel userInformation)
    {
        var result = await UsersDataAccess.SaveBasic(_sql, companyUUID, usersUUIDLoggedIn, userInformation);
        return result ?? new ResultsModel { isValid = false, Message = "Failed to save user information" };
    }

    public async Task<ResultsModel> SaveImage(string companyUUID, string usersUUIDLoggedIn, UserImageModel image)
    {
        try
        {
            // Save to database first
            var result = await UsersDataAccess.SaveImage(_sql, companyUUID, usersUUIDLoggedIn, image.UsersUUID, image.UsersImageBytes);
            
            if (result == null || !result.IsSuccess)
            {
                return new ResultsModel 
                { 
                    isValid = false, 
                    Message = result?.Message ?? "Failed to save image to database" 
                };
            }

            // Write file to disk
            if (!string.IsNullOrEmpty(result.ImageFileLocation))
            {
                try
                {
                    // Ensure directory exists
                    string? directoryPath = Path.GetDirectoryName(result.ImageFileLocation);
                    if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Write the image file
                    await File.WriteAllBytesAsync(result.ImageFileLocation, image.UsersImageBytes);
                }
                catch (Exception ex)
                {
                    // If file write fails, we could optionally revert the database change
                    return new ResultsModel 
                    { 
                        isValid = false, 
                        Message = $"Database updated but failed to write image file: {ex.Message}" 
                    };
                }
            }

            return new ResultsModel 
            { 
                UUID = image.UsersUUID,
                isValid = true, 
                Message = result.Message 
            };
        }
        catch (Exception ex)
        {
            return new ResultsModel 
            { 
                isValid = false, 
                Message = $"Error saving user image: {ex.Message}" 
            };
        }
    }

    public async Task<ResultsModel> Delete(string companyUUID, string usersUUIDLoggedIn, string userUUID)
    {
        var result = await UsersDataAccess.Delete(_sql, companyUUID, usersUUIDLoggedIn, userUUID);
        return result ?? new ResultsModel { isValid = false, Message = "Failed to delete user" };
    }

    public async Task<List<LookupBasicModel>?> GetEthnicity(string companyUUID, string usersUUIDLoggedin) 
    {
        var data = await UsersDataAccess.GetEthnicity(_sql, companyUUID, usersUUIDLoggedin);
        if (data == null) return null;
        return data.ToList();
    }

    public async Task<ResultsModel> GetUsersUUIDByIDNumber(string idNumber)
    {
        var data = await UsersDataAccess.GetUsersUUIDByIDNumber(_sql, idNumber);
        if (data == null) return new ResultsModel { isValid = false, Message= "ID Number not found" };
        return new ResultsModel { UUID = data, isValid = true };
    }

    public async Task<List<LookupBasicModel>?> GetGender(string companyUUID, string usersUUIDLoggedin)
    {
        var data = await UsersDataAccess.GetGender(_sql, companyUUID, usersUUIDLoggedin);
        if (data == null) return null;
        return data.ToList();
    }

    public async Task<List<NamesModel>?> GetUsersNamesSearch(string companyUUID, string UsersUUIDLoggedIn, string search, bool isIncludeTeam = false)
    {
        var data = await UsersDataAccess.GetUsersNamesSearch(_sql, companyUUID, UsersUUIDLoggedIn, search, isIncludeTeam);
        if (data == null) return null;
        return data.ToList();
    }

    

   
}
