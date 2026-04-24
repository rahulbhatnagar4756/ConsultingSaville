namespace Library.Base.Models.Employees.BusinessStructure
{
    public class EmployeeBaseModel
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? ParentsUUID { get; set; }
        public string? Parents { get; set; }
        public string? Description { get; set; }
        public int? IconsId { get; set; }
        public string? IconColor { get; set; }
        public string? Icon { get; set; }
        public int? OrderVal { get; set; } = 0;
        public int? Count { get; set; }
        public string? CountName { get; set; } = "";
    }

    //same as model above but with business Unit UUID and name and the business Unit type UUID and name and level uuid and name and 
    public class EmployeeFullModel : EmployeeBaseModel
    {
        public string? EmployeeBusinessUnitsUUID { get; set; }
        public string? EmployeeBusinessUnits { get; set; }
        public string? EmployeeBusinessUnitTypesUUID { get; set; }
        public string? EmployeeBusinessUnitTypes { get; set; }
        public string? EmployeeDepartmentsUUID { get; set; }
        public string? EmployeeDepartments { get; set; }
        public string? LevelsUUID { get; set; }
        public string? Levels { get; set; }
        public string? EmployeeJobsUUID { get; set; }
        public string? EmployeeJobs { get; set; }
    }

    public class EmployeeDepartmentBaseModel : EmployeeBaseModel
    {
        public string? EmployeeBusinessUnitsUUID { get; set; }
        public string? EmployeeBusinessUnits { get; set; }
        public string? EmployeeBusinessUnitTypesUUID { get; set; }
        public string? EmployeeBusinessUnitTypes { get; set; } 
    }
}
