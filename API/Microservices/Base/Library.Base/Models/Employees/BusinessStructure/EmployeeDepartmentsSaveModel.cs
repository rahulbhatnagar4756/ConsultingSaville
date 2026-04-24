namespace Library.Base.Models.Employees.BusinessStructure
{
    public class EmployeeDepartmentsSaveModel
    {
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDLoggedIn { get; set; }
        public string? EmployeeBusinessUnitsUUID { get; set; }
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? IconsId { get; set; }
        public string? IconColor { get; set; }
        public string? Icon { get; set; }
    }
}
