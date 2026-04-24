using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models.Employees
{
    public class EmployeeBasicInformationModel
    {
        public string? UserCompanyUUID { get; set; }
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDLoggedIn { get; set; }
        public string? UsersUUID { get; set; }
        public string? EmployeeNumber { get; set; }
        public DateTime? DateOfEmployment { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
