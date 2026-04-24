namespace Library.API.Goals.Models.Contracts;

public class ContractsBasicModel
{
    public string? ContractsUUID { get; set; }
    public string? ContractPeriodsUUID { get; set; }
    public string? ContractPeriods { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public bool isDepartmentTemplateSync { get; set; }
    public bool isIndividualTemplateSync { get; set; }
    public bool isActive { get; set; }

}

