using System.Text.Json;

namespace Library.Base.Models.Users;

public class UserLoginResponseModel
{
    public bool IsSuccessful { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? UsersUUID { get; set; }
    public string? CompanyUUIDLastLoggedIn { get; set; }

    public List<CompanyLoginModel> Company { get; set; } = new();
}

public class CompanyLoginModel
{
    public string CompanyUUID { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public List<RolesLoginModel> Roles { get; set; } = new();
}

public class RolesLoginModel
{
    public string Roles { get; set; } = string.Empty;
}