namespace Library.Base.Models.Users;

public class UserLoginTokenModel
{
    public bool IsSuccessful { get; set; } = false;
    public string? Message { get; set; } = string.Empty;
    public string? Token { get; set; }
}