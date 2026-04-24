namespace Library.Tools.Models.Tokens;

/// <summary>
/// returns list of claims as well as if the token is valid or not
/// </summary>
public class TokenClaimsResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<TokenClaims> Claims { get; set; } = new List<TokenClaims>();
}