using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library.Base.Services.Admin;
using Microsoft.AspNetCore.Authorization;

namespace Microservice.Base.Controllers.Admin;

[Route("api/Admin/[controller]")]
[ApiController]
public class IconsController : ControllerBase
{
    private readonly IIconsService _iconsService;
            public IconsController(IIconsService iconsService)
    {
        _iconsService = iconsService;
    }

    [HttpGet("All"), Authorize]
    public async Task<IActionResult> GetIcons()
    {
        var userClaims = User.Claims;
        if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);

        string CompanyUUID = User.FindFirst("Companyid")?.Value;
        string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

        if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);

        var icons = await _iconsService.GetIcons( new() { CompanyUUID= CompanyUUID
                                                        , UsersUUIDLoggedIn= UsersUUIDLoggedIn});
        if (icons == null || !icons.Any())
        {
            return StatusCode(StatusCodes.Status204NoContent);
        }
        return Ok(icons);
    }
}