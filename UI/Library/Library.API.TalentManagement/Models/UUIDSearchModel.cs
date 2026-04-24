using Library.API.Base.Models.Employees;

namespace Library.API.TalentManagement.Models;

public class UUIDSearchModel
{
    public string? UUID { get; set; }
    public EmployeeSearchModel? Search { get; set; }
}
