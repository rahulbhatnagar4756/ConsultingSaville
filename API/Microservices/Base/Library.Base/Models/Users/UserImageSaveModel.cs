namespace Library.Base.Models.Users;

public class UserImageSaveModel
{
    public string? CompanyUUID { get; set; }
    public string? UsersUUIDLoggedIn { get; set; }
    public string? UUID { get; set; }
    public byte[]? ImageBytes { get; set; }
    public string? ImageFileLocation { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}
