namespace Microservice.Base.DTO
{
    public class EmployeeSearchDTO
    {
        public string? Search { get; set; }
        public bool? isIncludeTeam { get; set; }

        public List<string>? Position { get; set; }
        public List<string>? Departments { get; set; }
        public List<string>? BusinessUnits { get; set; }
        public List<string>? BusinessUnitTypes { get; set; }
        public List<string>? Level { get; set; }
        public List<string>? CriticalRoles { get; set; }
        public List<string>? Disciplines { get; set; }
        public List<string>? YearsOfExperience { get; set; }
        public List<string>? Age { get; set; }
    }
}
