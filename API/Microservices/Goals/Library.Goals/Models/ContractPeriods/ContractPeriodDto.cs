using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.ContractPeriods
{
    /// <summary>
    /// (DTO) for a contract period.
    /// Used to transfer contract period details between layers.
    /// </summary>
    public class ContractPeriodDto
    {
        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the contract period.
        /// </summary>
        public string? UUID { get; set; }
        /// <summary>
        /// Gets or sets the name of the contract period.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the description of the contract period.
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
        public bool IsActive { get; set; }

        /// <summary>
        /// Converts this DTO into a <see cref="ContractPeriodSaveModel"/> for persistence.
        /// </summary>
        /// <param name="basic">The basic model containing company and user information.</param>
        /// <returns>A populated <see cref="ContractPeriodSaveModel"/> instance.</returns>
        public ContractPeriodSaveModel ContractPeriodSaveModel(BasicModel basic)
        {
            return new ContractPeriodSaveModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = this.UUID,
                Name = this.Name,
                Description = this.Description,
                DateStart = this.DateStart,
                DateEnd = this.DateEnd,
                Year = this.Year,
                IsActive = this.IsActive
            };
        }
    }

    /// <summary>
    /// Represents the model used to save a contract period.
    /// </summary>
    public class ContractPeriodSaveModel : IBasicModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the company.
        /// </summary>
        public string? CompanyUUID { get; set; }
        /// <summary>
        /// Gets or sets the UUID of the logged-in user.
        /// </summary>
        public string? UsersUUIDLoggedIn { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier of the contract period.
        /// </summary>
        public string? UUID { get; set; }
        /// <summary>
        /// Gets or sets the name of the contract period.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the description of the contract period.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Gets or sets the start date of the contract period.
        /// </summary>s
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
        public bool IsActive { get; set; }
    }
}
