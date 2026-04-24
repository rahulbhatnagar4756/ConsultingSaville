using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects
{
    public class SaveTokenModel
    {
        public string CompanyUUID { get; set; } = string.Empty;
        public string? UsersUUIDLoggedIn { get; set; }
        public string ProjectsUUID { get; set; } = string.Empty;
        public string UsersUUID { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }

    public class SaveTokenResultModel
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}