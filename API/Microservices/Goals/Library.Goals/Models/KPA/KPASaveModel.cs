using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.KPA
{
    public class KPASaveModel : IBasicModel
    {
        public string CompanyUUID { get; set; }
        public string UsersUUIDLoggedIn { get; set; }
        public string UsersUUID { get; set; }
        public string? KPAUUID { get; set; }
        public string PillarUUID { get; set; }
        public string EnterpriseStructureTypesUUID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public float Weights { get; set; } = 100;
        public string? TrackingUUID { get; set; }
    }
}
