using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TalentManagement.Models.SuccessionReadiness
{
    public class SuccessionReadinessRolesModel
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? Level { get; set; }
        public string? RequirementsForNextRole { get; set; }
    }
}
