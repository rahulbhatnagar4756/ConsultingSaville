using Library.Goals.Enumerables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.Contracts
{   
    public class ContractKPALinksSaveModel : IBasicModel
    {
        public string CompanyUUID { get; set; } = string.Empty;
        public string UsersUUIDLoggedIn { get; set; } = string.Empty;
        
        /// <summary>
        /// UUID of the KPA Link to update, null for new link
        /// </summary>
        public string? UUID { get; set; }
        
        /// <summary>
        /// UUID of the Contract Period this link belongs to
        /// </summary>
        public string? ContractPeriodsUUID { get; set; }
        
        /// <summary>
        /// Type of link (1=KPA, 2=KPI)
        /// </summary>
        public KPALinkTypes KPALinkTypesid { get; set; }
        
        /// <summary>
        /// UUID of the linked entity (KPA or KPI UUID)
        /// </summary>
        public string? LinkedUUID { get; set; }
        
        /// <summary>
        /// UUID of the KPA this link belongs to
        /// </summary>
        public string? KPAUUID { get; set; }
        
        /// <summary>
        /// Weight of the link
        /// </summary>
        public decimal Weight { get; set; } = 100.0m;
    }

    
}