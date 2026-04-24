using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Models.Employees.Jobs
{
    public class EmployeeJobEducationBaseModel
    {
        public string? UUID { get; set; }
        public string? EmployeeJobsUUID { get; set; }
        public string? EducationQualificationsUUID { get; set; }
        public string? Information { get; set; }
        public int? OrderVal { get; set; }
    }
}
