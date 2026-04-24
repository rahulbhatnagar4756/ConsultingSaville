using System.ComponentModel.DataAnnotations;

namespace Library.Base.Models.Employees;

public class EmployeeBasicGetModel
{
    [Required]
    public string? CompanyUUID { get; set; }
    [Required]
    public string? UsersUUIDLoggedIn { get; set; }
}
