using Library.Goals.Models.Contracts;
using Library.Goals.Services;
using Microservice.Base.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers.Contracts;

[Route("api/Goals/[controller]")]
[ApiController]
public class ContractsController : BaseGoalController
{
    private readonly ISecureService _secureService;
    private readonly IContractService _contractService;

    public ContractsController(ISecureService secureService, IContractService contractService)
    {
        _secureService = secureService;
        _contractService = contractService;
    }

    [HttpPost("Clones"), Authorize]
    public async Task<IActionResult> Clones([FromBody] ContractsCloneModel contractsClone)
    {
        var (basicModel, errorResult) = ValidateUserClaims();
        if (errorResult != null) return errorResult;

        var results = await _contractService.CloneContract(basicModel.CompanyUUID, basicModel.UsersUUIDLoggedIn, contractsClone);
        return Ok(results);
    }


    [HttpPost("GetContract"), Authorize]
    public async Task<IActionResult> GetContracts([FromBody] string contractsUUID)
    {
        var (basicModel, errorResult) = ValidateUserClaims();
        if (errorResult != null) return errorResult;

        var results = await _contractService.GetContract(basicModel, contractsUUID);
        return Ok(results);
    }

    [HttpPost("GetContractByUserContractPeriod"), Authorize]
    public async Task<IActionResult> GetContractByUserContractPeriod([FromBody] ContractUsersPeriodsModel? cup)
    {
        var (basicModel, errorResult) = ValidateUserClaims();
        if (errorResult != null) return errorResult;
        var results = await _contractService.GetContractByUserContractPeriod(basicModel, cup);
        return Ok(results);
    }

    [HttpPost("GetOrCreateLatestUserContract"), Authorize]
    public async Task<IActionResult> GetOrCreateLatestUserContract([FromBody] string usersUUID)
    {
        var (basicModel, errorResult) = ValidateUserClaims();
        if (errorResult != null) return errorResult;

        var results = await _contractService.GetOrCreateLatestUserContract(basicModel, usersUUID);
        return Ok(results);
    }
    
    [HttpPost("GetEmployeesBYUserUUID"), Authorize]
    public async Task<IActionResult> GetEmployeesBYUserUUID([FromBody] string usersUUID)
    {
        var (basicModel, errorResult) = ValidateUserClaims();
        if (errorResult != null) return errorResult;

        var results = await _contractService.GetEmployeesBYUserUUID(basicModel, usersUUID);
        return Ok(results);
    }
}
