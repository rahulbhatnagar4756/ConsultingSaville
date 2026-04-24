namespace Library.Frog.Models;

public class FrogChangeStatusModel
{
    public string? CompanyUUID { get; set; }
    public string? UsersUUIDLoggedIn { get; set; }
    public string? FrogsUUID { get; set; }
    public int? StatusId { get; set; }
}