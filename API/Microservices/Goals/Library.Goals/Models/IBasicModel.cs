namespace Library.Goals.Models
{
    public interface IBasicModel
    {
        string? CompanyUUID { get; set; }
        string? UsersUUIDLoggedIn { get; set; }
    }
}