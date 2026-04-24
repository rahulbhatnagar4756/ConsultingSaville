using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects
{
    public class ProjectValidationModel
    {
        public string? ProjectsUUID { get; set; }
        public string? ProjectName { get; set; }
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDCandidate { get; set; }
        public bool isSuccessful { get; set; }
        public string? Message { get; set; }
    }
}
