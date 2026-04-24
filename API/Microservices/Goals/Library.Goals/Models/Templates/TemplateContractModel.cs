namespace Library.Goals.Models.Templates;

public class TemplateContractModel
{
    public string? TemplatesUUID { get; set; }
    public string? CompanyUUID { get; set; }
    public string? UsersUUIDLoggedIn { get; set; }
    public string? ContractPeriodsUUID { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
}