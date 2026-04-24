namespace Library.API.Base.Models.Users;

public class UserBasicModel
{
    public string? UsersUUID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName
    {
        get
        {
            return $"{FirstName} {LastName}";
        }
    }
    public string? Email { get; set; }
    public string? UsersImageURL { get; set; }
    public List<RoleBasicModel> Roles { get; set; } = new();
}
