using Library.API.Goals.Models;
using Library.API.Goals.Models.Contracts;
using Library.API.Goals.Models.PerformanceReview;
using Library.API.Service;
using Library.Security.Services;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Services.Contracts;

public interface IContractsService
{
    Task<List<ResultsModel>?> CloneContract(ContractsCloneModel contractsClone);
    Task<ContractsModel?> GetContract(string contractsUUID);
    Task<ContractsModel?> GetContractByUserContractPeriod(ContractUsersPeriodsModel? cup);
    Task<ResultsModel?> GetOrCreateLatestUserContract(string usersUUID);
    Task<PerformanceReviewResponse?> DownloadEmployeesDetails(PerformanceReviewRequest request);
    Task<PerformanceReviewRequest> GetEmployeesBYUserUUID(string userUUID);


}

public class ContractsService : IContractsService
{
    private readonly IConfiguration _config;
    private readonly IAPIConnectService _aPIConnect;
    private readonly ITokenService _token;
    private readonly ISecurityService _securityService;

    private readonly string _urlBase;
    private readonly string _endPointContractClone = "Goals/Contracts/Clones";
    private readonly string _endPointContractGet = "Goals/Contracts/GetContract";
    private readonly string _endPointContractGetByUsersPeriod = "Goals/Contracts/GetContractByUserContractPeriod";
    private readonly string _endPointGetOrCreateLatestUserContract = "Goals/Contracts/GetOrCreateLatestUserContract";
    private readonly string _endPointGetEmployeesBYUserUUID = "Goals/Contracts/GetEmployeesBYUserUUID";

    private readonly string _endPointGeneratePerformanceReviewReport = "Assess/PerformanceReview/GeneratePerformanceReviewReport";


    public ContractsService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
    {
        _config = config;
        _aPIConnect = aPIConnect;
        _token = token;
        _securityService = securityService;
        _urlBase = _config.GetSection("API:Base:URL").Value;
    }


    public async Task<List<ResultsModel>?> CloneContract(ContractsCloneModel contractsClone)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        var Results = await _aPIConnect.PostAsync<List<ResultsModel>, ContractsCloneModel>(Token, $"{_urlBase}{_endPointContractClone}", contractsClone);
        return Results;
    }

    public async Task<PerformanceReviewResponse?> DownloadEmployeesDetails(PerformanceReviewRequest request)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        var Results = await _aPIConnect.PostAsync<PerformanceReviewResponse, PerformanceReviewRequest?>(Token, $"{_urlBase}{_endPointGeneratePerformanceReviewReport}", request);
        return Results;
    }

    public async Task<ContractsModel?> GetContract(string contractsUUID)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        var Results = await _aPIConnect.PostAsync<ContractsModel, string>(Token, $"{_urlBase}{_endPointContractGet}", contractsUUID);
        return Results;
    }

    public async Task<ContractsModel?> GetContractByUserContractPeriod(ContractUsersPeriodsModel? cup)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        var Results = await _aPIConnect.PostAsync<ContractsModel, ContractUsersPeriodsModel?>(Token, $"{_urlBase}{_endPointContractGetByUsersPeriod}", cup);
        return Results;
    }

    public async Task<PerformanceReviewRequest> GetEmployeesBYUserUUID(string userUUID)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        var Results = await _aPIConnect.PostAsync<PerformanceReviewRequest, string>(Token, $"{_urlBase}{_endPointGetEmployeesBYUserUUID}", userUUID);
        return Results;
    }

    public async Task<ResultsModel?> GetOrCreateLatestUserContract(string usersUUID)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        var Results = await _aPIConnect.PostAsync<ResultsModel, string>(Token, $"{_urlBase}{_endPointGetOrCreateLatestUserContract}", usersUUID);
        return Results;
    }

}
