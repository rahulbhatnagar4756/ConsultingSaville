using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Assess.Models.Projects
{
    public class CandidateAutoLoginResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? CompanyUUID { get; set; }
        public string? ProjectsUUID { get; set; }
        public string? UsersUUID { get; set; }
        public string? Token { get; set; }
    }
}
