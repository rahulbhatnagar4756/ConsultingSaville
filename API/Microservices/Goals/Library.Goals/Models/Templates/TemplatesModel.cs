using Library.Goals.Models.BASE;
using Library.Goals.Models.Contracts;

namespace Library.Goals.Models.Templates;

public class TemplatesModel
{
    public string? TemplatesUUID { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }

    public List<TemplateBusinessUnitDepartmentPositionsModel>? BusinessUnitDepartmentPositions { get; set; }
    public List<ContractsBasicModel>? Contracts { get; set; }
    public List<EmployeeBasicModel>? Employees { get; set; }
}
