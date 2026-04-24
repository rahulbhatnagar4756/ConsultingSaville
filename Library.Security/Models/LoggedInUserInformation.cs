using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Security.Models
{
    public class LoggedInUserInformation
    {
        public string? UsersUUID { get; set; }
        public string? CompanyUUID { get; set; }
        public List<LoggedInUserInformationRoles>? Roles { get; set; }
    }
}