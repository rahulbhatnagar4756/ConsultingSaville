using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.Contracts
{
    public class ContractKPASaveModel
    {
        /// <summary>
        /// UUID of the KPA to update, null for new KPA
        /// </summary>
        public string? UUID { get; set; }
        public string CompanyUUID { get; set; } = string.Empty;
        public string UsersUUIDLoggedIn { get; set; } = string.Empty;
        public string? UsersUUID { get; set; }
        
        /// <summary>
        /// UUID of the Contract Pillar this KPA belongs to
        /// </summary>
        public string ContractPillarsUUID { get; set; } = string.Empty;
        
        /// <summary>
        /// UUID of the Status for this KPA
        /// </summary>
        public string StatusUUID { get; set; } = string.Empty;
        public string? RatingPeriodsUUID { get; set; }

        /// <summary>
        /// Name of the KPA
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Description of the KPA
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Weight of the KPA within the pillar
        /// </summary>
        public decimal Weights { get; set; } = 100.0m;
        
        /// <summary>
        /// Whether the KPA is active
        /// </summary>
        public bool isActive { get; set; } = true;

        /// <summary>
        /// Templates UUID
        /// </summary>
        public string? TemplatesUUID { get; set; }
    }
}