namespace Library.Goals.Models.Templates;

public class TemplateBusinessUnitDepartmentPositionsSaveModel
{
    public string? UUID { get; set; }
    public string? CompanyUUID { get; set; }
    public string? UsersUUIDLoggedIn { get; set; }
    public string? TemplatesUUID { get; set; }
    public string? EmployeeBusinessUnitsUUID { get; set; }
    public string? EmployeeDepartmentsUUID { get; set; }
    public string? EmployeeJobsUUID { get; set; }
}