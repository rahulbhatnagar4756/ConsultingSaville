using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.Contracts
{
    public class ContractKPALinksModel
    {
        /// <summary>
        /// Unique identifier for the KPA Link
        /// </summary>
        public string? UUID { get; set; }
        
        /// <summary>
        /// UUID of the Contract Period this link belongs to
        /// </summary>
        public string? ContractPeriodsUUID { get; set; }
        
        /// <summary>
        /// Type of link (1=KPA, 2=KPI)
        /// </summary>
        public int KPALinkTypesid { get; set; }
        
        /// <summary>
        /// Description of the link type (KPA or KPI)
        /// </summary>
        public string? KPALinkTypes { get; set; }
        
        /// <summary>
        /// UUID of the linked entity (KPA or KPI UUID)
        /// </summary>
        public string? LinkedUUID { get; set; }
        
        /// <summary>
        /// UUID of the KPA this link belongs to
        /// </summary>
        public string? KPAUUID { get; set; }
        
        /// <summary>
        /// Name of the KPA this link belongs to
        /// </summary>
        public string? KPAName { get; set; }
        
        /// <summary>
        /// Name of the linked entity
        /// </summary>
        public string? LinkedName { get; set; }
        
        /// <summary>
        /// Description of the linked entity
        /// </summary>
        public string? LinkedDescription { get; set; }
        
        /// <summary>
        /// Weight of the link
        /// </summary>
        public decimal Weight { get; set; }
        
        /// <summary>
        /// Whether the link is active
        /// </summary>
        public bool isActive { get; set; }
        
        /// <summary>
        /// Whether the link is deleted
        /// </summary>
        public bool isDeleted { get; set; }
        
        /// <summary>
        /// When the link was created
        /// </summary>
        public DateTime? DateCreated { get; set; }
        
        /// <summary>
        /// UUID of the user who created the link
        /// </summary>
        public string? CreatedByUUID { get; set; }
        
        /// <summary>
        /// Name of the user who created the link
        /// </summary>
        public string? CreatedByName { get; set; }
        
        /// <summary>
        /// For KPI links: UUID of the user who owns the KPI
        /// </summary>
        public string? LinkedUserUUID { get; set; }
        
        /// <summary>
        /// For KPI links: Name of the user who owns the KPI
        /// </summary>
        public string? LinkedUserName { get; set; }
        
        /// <summary>
        /// For KPI links: Email of the user who owns the KPI
        /// </summary>
        public string? LinkedUserEmail { get; set; }
        
        /// <summary>
        /// For KPI links: Employee number of the user who owns the KPI
        /// </summary>
        public string? LinkedUserEmployeeNumber { get; set; }
    }
}