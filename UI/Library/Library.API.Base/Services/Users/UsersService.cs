using Library.API.Base.Models;
using Library.API.Base.Models.Users;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using OneCoreAssessUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Services.Users;

public interface IUsersService
{
    Task<List<LookupBasicModel>?> GetEthnicity();
    Task<List<LookupBasicModel>?> GetGender();
    Task<LoggedInInformationModel> GetLoggedInInformation();
    Task<UserBasicInformationModel?> GetUserBasicInformationByUUID(string usersUUID);
    Task<UserBasicModel?> GetUserByUUID(string usersUUID);
    Task<UserBasicModel?> GetUserFromToken();
    Task<List<NamesModel>?> GetUsersNamesSearch(string search, bool isIncludeTeam = false);
    Task<ResultsModel> GetUsersUUIDByIDNumber(string idNumber);
    Task<ResultsModel?> SetUserBasicInformationByUUID(UserBasicInformationModel basicInformation);
    Task<ResultsModel?> SaveImage(UserImageModel imageModel);
    Task<ResultsModel?> Delete(string userUUID);
    Task<ResultsModel?> SaveBasic(UserBasicInformationModel basicInformation);
}

public class UsersService : IUsersService
{
    private readonly IConfiguration _config;
    private readonly IAPIConnectService _aPIConnect;
    private readonly ITokenService _token;
    private readonly ISecurityService _securityService;
    private readonly ICompanyService _companyService;
    private readonly IUserRolesService _userRolesService;
    private readonly string _urlBase;
    private readonly string _endPointUsers = "Users/UserByUUID";
    private readonly string _endPointUserBasicInformation = "Users/UserBasicInformationByUUID";
    private readonly string _endPointGender = "Users/Gender";
    private readonly string _endPointEthnicity = "Users/Ethnicity";
    private readonly string _endPointIDNumber = "Users/UserUUIDByIDNumber";
    private readonly string _endPointNamesSearch = "Users/NamesSearch";
    private readonly string _endPointSaveImage = "Users/SaveImage";
    private readonly string _endPointSaveBasic = "Users/SaveBasic";
    private readonly string _endPointDelete = "Users/Delete";

    public UsersService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token
        , ISecurityService securityService, ICompanyService companyService, IUserRolesService userRolesService)
    {
        _config = config;
        _aPIConnect = aPIConnect;
        _token = token;
        _securityService = securityService;
        _companyService = companyService;
        _userRolesService = userRolesService;
        _urlBase = _config.GetSection("API:Base:URL").Value;
    }

    /// <summary>
    /// get the current logged in users information
    /// </summary>
    /// <returns></returns>
    public async Task<UserBasicModel?> GetUserFromToken()
    {
        var user = await _securityService.GetUserInformation();
        if (user == null) return null;
        return await GetUserByUUID(user.UsersUUID);
    }

    /// <summary>
    /// get the company information by the company UUID
    /// </summary>
    /// <param name="companyUUID"></param>
    /// <returns></returns>
    public async Task<UserBasicModel?> GetUserByUUID(string usersUUID)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;

        UserBasicModel? Results
            = await _aPIConnect.GetAsync<UserBasicModel>(Token, $"{_urlBase}{_endPointUsers}/{usersUUID}");

        return Results;
    }

    public async Task<LoggedInInformationModel> GetLoggedInInformation()
    {
        LoggedInInformationModel LoggedInInformation = new();

        var userTask = GetUserFromToken();
        var companyTask = _companyService.GetCompanyFromToken();

        await Task.WhenAll(userTask, companyTask);

        LoggedInInformation.User = await userTask;
        LoggedInInformation.Company = await companyTask;

        return LoggedInInformation;
    }

    public async Task<UserBasicInformationModel?> GetUserBasicInformationByUUID(string usersUUID)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;

        UserBasicInformationModel? Results
            = await _aPIConnect.GetAsync<UserBasicInformationModel>(Token, $"{_urlBase}{_endPointUserBasicInformation}/{usersUUID}");

        return Results;
    }

    public async Task<ResultsModel?> SetUserBasicInformationByUUID(UserBasicInformationModel basicInformation)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;

        ResultsModel? Results
            = await _aPIConnect.PostAsync<ResultsModel, UserBasicInformationModel>(Token, $"{_urlBase}{_endPointUserBasicInformation}", basicInformation);
        return Results;
    }


    public async Task<ResultsModel?> SaveBasic(UserBasicInformationModel basicInformation)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;

        ResultsModel? Results
            = await _aPIConnect.PostAsync<ResultsModel, UserBasicInformationModel>(Token, $"{_urlBase}{_endPointSaveBasic}", basicInformation);
        return Results;
    }

    public async Task<ResultsModel?> SaveImage(UserImageModel imageModel)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;

        ResultsModel? Results
            = await _aPIConnect.PostAsync<ResultsModel, UserImageModel>(Token, $"{_urlBase}{_endPointSaveImage}", imageModel);
        return Results;
    }

    public async Task<ResultsModel?> Delete(string userUUID)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;

        ResultsModel? Results
            = await _aPIConnect.PostAsync<ResultsModel, string>(Token, $"{_urlBase}{_endPointDelete}", userUUID);
        return Results;
    }

    public async Task<List<LookupBasicModel>?> GetGender()
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;

        List<LookupBasicModel>? Results
            = await _aPIConnect.GetAsync<List<LookupBasicModel>>(Token, $"{_urlBase}{_endPointGender}");

        return Results;
    }

    public async Task<List<LookupBasicModel>?> GetEthnicity()
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;

        List<LookupBasicModel>? Results
            = await _aPIConnect.GetAsync<List<LookupBasicModel>>(Token, $"{_urlBase}{_endPointEthnicity}");
        return Results;
    }

    public async Task<ResultsModel> GetUsersUUIDByIDNumber(string idNumber)  
    {
        string? Token = await _token.GetToken();
        if (Token == null) return default(ResultsModel);
        ResultsModel? Results
            = await _aPIConnect.GetAsync<ResultsModel>(Token, $"{_urlBase}{_endPointIDNumber}/{idNumber}");
        return Results ;
    }

    public async Task<List<NamesModel>?> GetUsersNamesSearch(string search, bool isIncludeTeam = false)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;

        UserSearchNameModel userSearch = new()
        {
            Search = search,
            IsIncludeTeam = isIncludeTeam
        };

        List<NamesModel>? Results
            = await _aPIConnect.PostAsync<List<NamesModel>, UserSearchNameModel>(Token, $"{_urlBase}{_endPointNamesSearch}", userSearch);
        return Results;
    }
}
