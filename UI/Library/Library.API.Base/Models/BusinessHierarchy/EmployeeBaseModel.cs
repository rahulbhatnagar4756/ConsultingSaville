namespace Library.API.Base.Models.BusinessHierarchy;

public class EmployeeBaseModel
{
    public string? UUID { get; set; }
    public string? ParentsUUID { get; set; }
    public string? Parents { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? IconsId { get; set; }
    public string? IconColor { get; set; }
    public string? Icon { get; set; }
    public int? OrderVal { get; set; } = 0;
    public int? Count { get; set; }
    public string? CountName { get; set; } = "";
}

public class EmployeeDepartmentBaseModel : EmployeeBaseModel 
{
    public string? EmployeeBusinessUnitTypesUUID { get; set; }
    public string? EmployeeBusinessUnitTypes { get; set; }
    public string? EmployeeBusinessUnitsUUID { get; set; }
    public string? EmployeeBusinessUnits { get; set; }
    public string? FullName { get { return $"{EmployeeBusinessUnits} - {Name}"; } }
}

