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

namespace Library.API.Base.Services.BusinessHierarchy;

public interface IDepartmentService
{
    Task<ResultsModel> Delete(string UUID);
    Task<List<EmployeeDepartmentBaseModel>?> GetDepartments();
    Task<ResultsModel> Save(EmployeeDepartmentBaseModel businessUnitType);
}

public class DepartmentService : IDepartmentService
{
    private readonly IConfiguration _config;
    private readonly IAPIConnectService _aPIConnect;
    private readonly ITokenService _token;
    private readonly ISecurityService _securityService;
    private readonly string _urlBase;
    private readonly string _endPointDepartments = "Employee/Departments/All";
    private readonly string _endPointDepartmentsSave = "Employee/Departments/Save";
    private readonly string _endPointDepartmentsDelete = "Employee/Departments/Delete";

    public DepartmentService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
    {
        _config = config;
        _aPIConnect = aPIConnect;
        _token = token;
        _securityService = securityService;
        _urlBase = _config.GetSection("API:Base:URL").Value;
    }

    public async Task<List<EmployeeDepartmentBaseModel>?> GetDepartments()
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        var response = await _aPIConnect.GetAsync<List<EmployeeDepartmentBaseModel>>(Token, $"{_urlBase}{_endPointDepartments}");
        return response;
    }

    //add save and delete methods 
    public async Task<ResultsModel> Save(EmployeeDepartmentBaseModel businessUnitType)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return new ResultsModel { isValid = false, Message = "Authentication failed." };
        var response = await _aPIConnect.PostAsync<ResultsModel, EmployeeDepartmentBaseModel>(Token, $"{_urlBase}{_endPointDepartmentsSave}", businessUnitType);
        return response;
    }

    public async Task<ResultsModel> Delete(string UUID)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return new ResultsModel { isValid = false, Message = "Authentication failed." };
        var response = await _aPIConnect.PostAsync<ResultsModel, string>(Token, $"{_urlBase}{_endPointDepartmentsDelete}", UUID);
        return response;
    }
}