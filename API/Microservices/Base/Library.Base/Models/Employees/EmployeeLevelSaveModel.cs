namespace Library.Base.Models.Employees
{
    public class EmployeeLevelSaveModel
    {
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDLoggedIn { get; set; }
        public string? UUID { get; set; }
        public string Name { get; set; }  
        public string? Description { get; set; }
        public int LevelOrder { get; set; } = 10;
    }
}