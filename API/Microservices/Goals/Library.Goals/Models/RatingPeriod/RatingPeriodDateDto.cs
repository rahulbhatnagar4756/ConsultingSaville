using System.ComponentModel.DataAnnotations;

namespace Library.Goals.Models.RatingPeriod
{
    /// <summary>
    /// DTO for Rating Period Dates - Specific instances (Q1 2025, Jan 2025, 2025)
    /// </summary>
    public class RatingPeriodDateDto
    {
        public string? UUID { get; set; }
        public string? RatingPeriodsUUID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty; // "Q1 2025", "Jan 2025", "2025"

        [Required]
        public DateTime DateStart { get; set; } // Start of evaluation period

        public DateTime? DateEnd { get; set; } // End of evaluation period

        [Required]
        public DateTime DateOpen { get; set; } // When rating becomes available

        public DateTime? DateClose { get; set; } // When rating window closes

        public bool isActive { get; set; } = true; // Rating window open/closed
        public bool isDeleted { get; set; }

        /// <summary>
        /// Converts DTO to Save Model for database operations
        /// </summary>
        public RatingPeriodDateSaveModel RatingPeriodDateSaveModel(BasicModel basic)
        {
            return new RatingPeriodDateSaveModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = this.UUID,
                RatingPeriodsUUID = this.RatingPeriodsUUID ?? string.Empty,
                Name = this.Name,
                DateStart = this.DateStart,
                DateEnd = this.DateEnd,
                DateOpen = this.DateOpen,
                DateClose = this.DateClose,
                isActive = this.isActive
            };
        }
    }
}
