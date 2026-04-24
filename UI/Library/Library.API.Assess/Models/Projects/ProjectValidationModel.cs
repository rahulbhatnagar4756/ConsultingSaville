using System;

namespace Library.API.Assess.Models.Projects
{
    public class ProjectValidationModel
    {
        public string? ProjectsUUID { get; set; }
        public string? ProjectName { get; set; }
        public string? CompanyUUID { get; set; }
        public bool isSuccessful { get; set; }
        public string? Message { get; set; }
    }
}