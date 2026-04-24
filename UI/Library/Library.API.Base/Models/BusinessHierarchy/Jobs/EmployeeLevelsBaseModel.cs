using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models.BusinessHierarchy.Jobs
{
    public class EmployeeLevelsBaseModel
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int LevelOrder { get; set; }
    }
}
