using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.ContractPeriod
{
    /// <summary>
    /// Represents a contract period entity
    /// </summary>
    public class ContractPeriod
    {
        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the contract period.
        /// </summary>
        public string UUID { get; set; }
        /// <summary>
        /// Gets or sets the name of the contract period.
        /// Defaults to an empty string if not specified.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets an optional description of the contract period.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Gets or sets the start date of the contract period.
        /// </summary>
        public DateTime DateStart { get; set; }
        /// <summary>
        /// Gets or sets the end date of the contract period.
        /// </summary>
        public DateTime DateEnd { get; set; }
        /// <summary>
        /// Gets or sets the year associated with the contract period.
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the contract period is active.
        /// </summary>
        public bool isActive { get; set; }
    }
}
