namespace Library.Assess.Models.Projects;

/// <summary>
/// Model for validating token data against database
/// </summary>
public class TokenValidationModel
{
    public string CompanyUUID { get; set; } = string.Empty;
    public string ProjectsUUID { get; set; } = string.Empty;
    public string UsersUUID { get; set; } = string.Empty; 
}
