using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.RatingPeriod
{
    /// <summary>
    /// DTO for Rating Period data transfer - Period Types (Month, Quarter, Year)
    /// </summary>
    public class RatingPeriodDto
    {
        public string? UUID { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // "Quarter", "Month", "Year"

        [Required]
        [StringLength(200)]
        public string DisplayName { get; set; } = string.Empty; // "Qtr", "Mth", "Yr"

        public bool isDeleted { get; set; }

        /// <summary>
        /// Converts DTO to Save Model for database operations
        /// </summary>
        public RatingPeriodSaveModel RatingPeriodSaveModel(BasicModel basic)
        {
            return new RatingPeriodSaveModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = this.UUID,
                Name = this.Name,
                DisplayName = this.DisplayName
            };
        }
    }
}
