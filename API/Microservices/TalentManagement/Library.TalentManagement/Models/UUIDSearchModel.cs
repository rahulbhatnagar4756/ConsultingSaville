using Library.Base.Models.Employees;

namespace Library.TalentManagement.Models;

public class UUIDSearchModel
{
    public string? UUID { get; set; }
    public EmployeeSearchModel? Search { get; set; }
}
