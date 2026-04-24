using Library.API.Base.Models.BusinessHierarchy.Jobs;
using Library.API.Base.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models.Employees
{
    public class EmployeeBasicModel
    {
        public UserBasicInformationModel? UserInformation { get; set; }
        public List<EmployeePositionModel>? EmployeeHierarchy { get; set; }
        public EmployeeBasicInformationModel? EmployeeInformation { get; set; }
        
        public string PositionName()
        {
            if (EmployeeHierarchy == null || EmployeeHierarchy.Count == 0) return string.Empty;
            return EmployeeHierarchy.FirstOrDefault()?.EmployeeJobs ?? string.Empty;
        }
    }

}
