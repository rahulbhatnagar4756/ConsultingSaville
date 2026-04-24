using Library.Base.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Library.Base.Models.Users;
using Library.Base.Models;

namespace Microservice.Base.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;
    private string CompanyUUID = "";
    private string UsersUUIDLoggedIn = "";
    private bool isClaimsSuccess = false;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }


    [HttpPost("NamesSearch"), Authorize]
    public async Task<List<NamesModel>?> GetUsersNamesSearch([FromBody] UserSearchNameModel search)
    {
        GetCompanyUserFromClaims();
        if (!isClaimsSuccess) return null;
        List<NamesModel>? data = await _usersService.GetUsersNamesSearch(CompanyUUID, UsersUUIDLoggedIn, search.Search, search.IsIncludeTeam);
        if (data == null) return null;
        return data;
    }


    private void GetCompanyUserFromClaims()
    {
        isClaimsSuccess = false;
        if (User == null) return;
        var userClaims = User.Claims;
        if (userClaims == null) return;

        isClaimsSuccess = true;
        CompanyUUID = User.FindFirst("Companyid")?.Value ?? User.FindFirst("CompanyUUID")?.Value ??  "";
        UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value ?? User.FindFirst("UsersUUID")?.Value ?? "";
    }

    [HttpGet("UserByUUID/{UsersUUID}"), Authorize]
    public async Task<IActionResult> GetUserByUUID(string usersUUID)
    {
        GetCompanyUserFromClaims();
        if (!isClaimsSuccess) return StatusCode(StatusCodes.Status401Unauthorized);
        var data = await _usersService.GetUserByUUID(CompanyUUID, UsersUUIDLoggedIn, usersUUID);
        if (data == null) return NotFound();
        return Ok( data);
    }


    [HttpGet("UserBasicInformationByUUID/{UsersUUID}"), Authorize]
    public async Task<IActionResult> GetUserBasicInformationByUUID(string usersUUID)
    {
        GetCompanyUserFromClaims();
        if (!isClaimsSuccess) return StatusCode(StatusCodes.Status401Unauthorized);
        var data = await _usersService.GetUserBasicInformationByUUID(CompanyUUID, UsersUUIDLoggedIn, usersUUID);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpPost("UserBasic"), Authorize]
    public async Task<IActionResult> GetUserBasic([FromBody] string usersUUID)
    {
        GetCompanyUserFromClaims();
        if (!isClaimsSuccess) return StatusCode(StatusCodes.Status401Unauthorized);
        var data = await _usersService.GetUserBasic(CompanyUUID, UsersUUIDLoggedIn, usersUUID);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpPost("UserBasicInformationByUUID"), Authorize]
    public async Task<IActionResult> GetUserBasicInformationByUUID([FromBody] UserBasicInformationModel? userInformation)
    {
        GetCompanyUserFromClaims();
        if (!isClaimsSuccess) return StatusCode(StatusCodes.Status401Unauthorized);
        var data = await _usersService.SetUserBasicInformationByUUID(CompanyUUID, UsersUUIDLoggedIn, userInformation);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpPost("SaveBasic"), Authorize]
    public async Task<IActionResult> SaveBasic([FromBody] UserBasicInformationModel? userInformation)
    {
        GetCompanyUserFromClaims();
        if (!isClaimsSuccess) return StatusCode(StatusCodes.Status401Unauthorized);
        var data = await _usersService.SaveUserBasic(CompanyUUID, UsersUUIDLoggedIn, userInformation);
        if (data == null) return NotFound();
        return Ok(data);
    }

    /// <summary>
    /// gets the users UUID by ID number
    /// </summary>
    /// <param name="idNumber"></param>
    /// <returns></returns>
    [HttpGet("UserUUIDByIDNumber/{idNumber}"), Authorize]
    public async Task<IActionResult> GetUsersUUIDByIDNumber(string idNumber)
    {
        var data = await _usersService.GetUsersUUIDByIDNumber(idNumber);
        if (data == null) return NotFound();
        return Ok(data);
    }


    [HttpGet("Gender"), Authorize]
    public async Task<IActionResult> GetGender()
    {
        GetCompanyUserFromClaims();

        if (!isClaimsSuccess) return StatusCode(StatusCodes.Status401Unauthorized);
        var data = await _usersService.GetGender(CompanyUUID, UsersUUIDLoggedIn);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpGet("Ethnicity"), Authorize]
    public async Task<IActionResult> GetEthnicity()
    {
        GetCompanyUserFromClaims();
        if (!isClaimsSuccess) return StatusCode(StatusCodes.Status401Unauthorized);
        var data = await _usersService.GetEthnicity(CompanyUUID, UsersUUIDLoggedIn);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpPost("SaveImage"), Authorize]
    public async Task<IActionResult> SaveUserImage([FromBody] UserImageModel image)
    {
        GetCompanyUserFromClaims();
        if (!isClaimsSuccess) return StatusCode(StatusCodes.Status401Unauthorized);
        
        if (image.UsersImageBytes == null || image.UsersImageBytes.Length == 0)
            return BadRequest("Image data is required");

        var result = await _usersService.SaveImage(CompanyUUID, UsersUUIDLoggedIn, image);
        
        if (!result.isValid.HasValue || !result.isValid.Value)
            return BadRequest(result.Message);
            
        return Ok(result);
    }

    [HttpPost("Delete"), Authorize]
    public async Task<IActionResult> DeleteUser([FromBody] string userUUID)
    {
        GetCompanyUserFromClaims();
        if (!isClaimsSuccess) return StatusCode(StatusCodes.Status401Unauthorized);
        
        var result = await _usersService.Delete(CompanyUUID, UsersUUIDLoggedIn, userUUID);
        
        if (!result.isValid.Value)
            return BadRequest(result.Message);
            
        return Ok(result);
    }

}
