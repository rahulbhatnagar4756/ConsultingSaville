namespace Library.Base.Models.Employees.BusinessStructure
{
    public class EmployeeBusinessUnitsModel
    {
        public string? UUID { get; set; }
        public string? CompanyUUID { get; set; }
        public string? EmployeeBusinessUnitTypesUUID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? IconsId { get; set; }
        public string? Icon { get; set; }
        public string? IconColor { get; set; }
        public bool IsDeleted { get; set; }
    }
}
