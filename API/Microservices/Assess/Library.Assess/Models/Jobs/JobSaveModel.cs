namespace Library.Assess.Models.Jobs;

public class JobSaveModel
{
    public string? CompanyUUID { get; set; }
    public string? UsersUUIDLoggedIn { get; set; }
    public string? UUID { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string? JobType { get; set; }
    public int isCritical { get; set; } = 0;
}
