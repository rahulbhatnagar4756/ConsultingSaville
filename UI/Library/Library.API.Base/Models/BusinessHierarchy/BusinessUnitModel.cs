using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models.BusinessHierarchy;

public class BusinessUnitsModel
{
    public string? UUID { get; set; }
    public string? BusinessUnitTypesUUID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int CountDepartments { get; set; } = 0;
}

public class BusinessUnitTypesModel
{
    public string? UUID { get; set; }
    public string? Name { get; set; }
    public int CountBusinessUnit { get; set; } = 0;

}

public class DepartmentsModel { 
    public string? UUID { get; set; }
    public string? BusinessUnitUUID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int CountPosition { get; set; } = 0;
}

