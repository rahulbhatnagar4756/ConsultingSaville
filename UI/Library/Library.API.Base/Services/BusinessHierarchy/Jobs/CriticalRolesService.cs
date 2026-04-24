using Library.API.Base.Models;
using Library.API.Base.Models.BusinessHierarchy;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Services.BusinessHierarchy.Jobs;

public interface ICriticalRolesService
{
    Task<List<EmployeeBaseModel>?> GetCriticalRoles();
    Task<ResultsModel> Save(EmployeeBaseModel criticalRole);
    Task<ResultsModel> Delete(string UUID);
}

public class CriticalRolesService : ICriticalRolesService
{
    private readonly IConfiguration _config;
    private readonly IAPIConnectService _aPIConnect;
    private readonly ITokenService _token;
    private readonly ISecurityService _securityService;
    private readonly string _urlBase;
    private readonly string _endPointCriticalRoles = "Employee/CriticalRoles/All";
    private readonly string _endPointCriticalRolesSave = "Employee/CriticalRoles/Save";
    private readonly string _endPointCriticalRolesDelete = "Employee/CriticalRoles/Delete";

    public CriticalRolesService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
    {
        _config = config;
        _aPIConnect = aPIConnect;
        _token = token;
        _securityService = securityService;
        _urlBase = _config.GetSection("API:Base:URL").Value;
    }

    public async Task<List<EmployeeBaseModel>?> GetCriticalRoles()
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;

        var response = await _aPIConnect.GetAsync<List<EmployeeBaseModel>>(Token, $"{_urlBase}{_endPointCriticalRoles}");
        return response;
    }

    public async Task<ResultsModel> Save(EmployeeBaseModel criticalRole)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return new ResultsModel { isValid = false, Message = "Authentication failed." };

        var response = await _aPIConnect.PostAsync<ResultsModel, EmployeeBaseModel>(Token, $"{_urlBase}{_endPointCriticalRolesSave}", criticalRole);
        return response ?? new ResultsModel { isValid = false, Message = "Save operation failed." };
    }

    public async Task<ResultsModel> Delete(string UUID)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return new ResultsModel { isValid = false, Message = "Authentication failed." };

        var response = await _aPIConnect.PostAsync<ResultsModel, string>(Token, $"{_urlBase}{_endPointCriticalRolesDelete}", UUID);
        return response ?? new ResultsModel { isValid = false, Message = "Delete operation failed." };
    }
}
