using Library.API.Goals.Models.BASE;
using Library.API.Goals.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.Templates;

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
