namespace Library.API.Goals.Models.Contracts;

public class ContractsCloneModel
{
    public string? ContractsUUID { get; set; }
    public string? TemplatesUUID { get; set; }
    public List<string>? UsersUUIDs { get; set; }
}

