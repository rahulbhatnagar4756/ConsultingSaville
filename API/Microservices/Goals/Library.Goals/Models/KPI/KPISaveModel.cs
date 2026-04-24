using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.KPI
{
    public class KPISaveModel : IBasicModel
    {
        public string? UUID { get; set; }
        public string CompanyUUID { get; set; }
        public string UsersUUIDLoggedIn { get; set; }
        public string UsersUUID { get; set; }
        public string KPAUUID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? StatusUUID { get; set; } 
        public string? ToleranceSetsUUID { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public decimal Target { get; set; } = 100;
        public string? TargetName { get; set; }
        public decimal Weights { get; set; } = 100;
        public string? TrackingUUID { get; set; }
        public bool isScoreProcessing { get; set; } = true;
    }
}
