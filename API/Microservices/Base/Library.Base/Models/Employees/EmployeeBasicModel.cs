using Library.Base.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Models.Employees
{
    public class EmployeeBasicModel
    {
        public UserBasicInformationModel? UserInformation { get; set; }
        public List<EmployeeHierarchyModel>? EmployeeHierarchy { get; set; }
        public EmployeeBasicInformationModel? EmployeeInformation { get; set; }
               
    }
}
