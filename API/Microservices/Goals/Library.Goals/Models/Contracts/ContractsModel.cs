using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.Contracts
{
    public class ContractsModel
    {
        /// <summary>
        /// Unique identifier for the contract.
        /// </summary>
        public string? ContractsUUID { get; set; }
        
        /// <summary>
        /// Unique identifier for the associated company.
        /// </summary>
        public string? CompanyUUID { get; set; }
        
        /// <summary>
        /// Name of the company associated with the contract.
        /// </summary>
        public string? Company { get; set; }
        
        /// <summary>
        /// Unique identifier for the user associated with the contract.
        /// </summary>
        public string? UsersUUID { get; set; }
        
        /// <summary>
        /// Unique identifier for the template associated with the contract.
        /// </summary>
        public string? TemplatesUUID { get; set; }
        
        /// <summary>
        /// Name of the contract owner or responsible person.
        /// </summary>
        public string? ContractOwnerName { get; set; }
        
        /// <summary>
        /// Unique identifier for the contract period.
        /// </summary>
        public string? ContractPeriodsUUID { get; set; }
        
        /// <summary>
        /// Display name or description of the contract period.
        /// </summary>
        public string? ContractPeriods { get; set; }
        
        /// <summary>
        /// The start date of the contract.
        /// </summary>
        public DateTime? DateStart { get; set; }
        
        /// <summary>
        /// The end date of the contract.
        /// </summary>
        public DateTime? DateEnd { get; set; }
        
        /// <summary>
        /// Indicates whether the current user is linked to the contract.
        /// </summary>
        public bool isUser { get; set; }
        
        /// <summary>
        /// Indicates whether this contract is a template.
        /// </summary>
        public bool isTemplate { get; set; }
        
        /// <summary>
        /// Indicates whether the contract is currently active.
        /// </summary>
        public bool isActive { get; set; }
        
        /// <summary>
        /// Indicates whether the contract is marked as deleted.
        /// </summary>
        public bool isDeleted { get; set; }

        /// <summary>
        /// Enterprise structures associated with the contract.
        /// </summary>
        public List<ContractEnterpriseStructureModel>? EnterpriseStructures { get; set; }
    }
}