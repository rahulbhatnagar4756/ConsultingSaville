using Library.API.Base.Models;
using Library.API.Base.Models.BusinessHierarchy.Jobs;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Services.BusinessHierarchy.Jobs;

public interface ILevelsService
{
    Task<ResultsModel> DeleteLevels(string UUID);
    Task<List<EmployeeLevelsBaseModel>?> GetLevels();
    Task<ResultsModel?> SaveLevels(EmployeeLevelsBaseModel levels);
}

public class LevelsService : ILevelsService
{
    private readonly IConfiguration _config;
    private readonly IAPIConnectService _aPIConnect;
    private readonly ITokenService _token;
    private readonly ISecurityService _securityService;
    private readonly string _urlBase;
    private readonly string _endPointLevels = "Employee/Levels/All";
    private readonly string _endPointLevelsSave = "Employee/Levels/Save";
    private readonly string _endPointLevelsDelete = "Employee/Levels/Delete";

    public LevelsService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
    {
        _config = config;
        _aPIConnect = aPIConnect;
        _token = token;
        _securityService = securityService;
        _urlBase = _config.GetSection("API:Base:URL").Value;
    }

    public async Task<List<EmployeeLevelsBaseModel>?> GetLevels()
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        var response = await _aPIConnect.GetAsync<List<EmployeeLevelsBaseModel>>(Token, $"{_urlBase}{_endPointLevels}");
        return response;
    }

    public async Task<ResultsModel?> SaveLevels(EmployeeLevelsBaseModel levels)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return new ResultsModel { isValid = false, Message = "Authentication failed." };
        var response = await _aPIConnect.PostAsync<ResultsModel, EmployeeLevelsBaseModel>(Token, $"{_urlBase}{_endPointLevelsSave}", levels);
        return response;
    }

    public async Task<ResultsModel> DeleteLevels(string UUID)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return new ResultsModel { isValid = false, Message = "Authentication failed." };
        var response = await _aPIConnect.PostAsync<ResultsModel, string>(Token, $"{_urlBase}{_endPointLevelsDelete}", UUID);
        return response;
    }
}