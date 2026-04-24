using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.EnterpriseStructures
{
    public class EnterpriseStructureWeightsModel
    {
        public string? UUID { get; set; }
        public string? EnterpriseStructureTypesUUID { get; set; }
        public string? EnterpriseStructureTypes { get; set; }
        public string? EmployeeLevelsUUID { get; set; }
        public string? EmployeeLevels { get; set; }
        public decimal Weight { get; set; } 
    }

    public class EnterpriseStructureWeightsSaveModel : IBasicModel
    {
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDLoggedIn { get; set; }
        public string? UUID { get; set; }
        public string? EnterpriseStructureTypesUUID { get; set; }
        public string? EmployeeLevelsUUID { get; set; }
        public decimal Weight { get; set; }
    }
}
