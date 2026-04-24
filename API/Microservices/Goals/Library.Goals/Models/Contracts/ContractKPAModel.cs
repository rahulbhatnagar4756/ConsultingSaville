namespace Library.Goals.Models.Contracts;

public class ContractKPAModel
{
    public string? UUID { get; set; }
    public string? ContractPillarsUUID { get; set; }
    public string? StatusUUID { get; set; }
    public string? Status { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Weight { get; set; }
    public bool isActive { get; set; }
    public List<ContractKPIModel>? KPI { get; set; }
    // New class for Linked property
    public List<ContractKPALinkedModel>? Linked { get; set; }
}

public class ContractKPALinkedModel
{
    public string? UUID { get; set; }
    public string? KPALinkUUID { get; set; }
    public string? CompanyUUID { get; set; }
    public string? KPALinkTypes { get; set; } // "KPI" or "KPA"
    public string? KPALinkTypesid { get; set; } // "KPI" or "KPA"
    public string? UsersUUID { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? IDNumber { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Weight { get; set; }
}

public class ContractKPALinkedSaveModel
{
    public string? UUID { get; set; }
    public string? KPAUUID { get; set; }
    public string? KPALinkUUID { get; set; }
    public string? KPALinkTypes { get; set; } // "KPI" or "KPA"
    public string? KPALinkTypesid { get; set; } // "KPI" or "KPA"
    public decimal? Weight { get; set; }
}