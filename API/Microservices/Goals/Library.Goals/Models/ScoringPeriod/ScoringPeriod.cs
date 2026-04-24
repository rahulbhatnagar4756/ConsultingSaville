using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Goals.Models.ScoringPeriod
{
    /// <summary>
    /// DTO for Scoring Periods (Recurring annual scoring windows)
    /// </summary>
    public class ScoringPeriodDto
    {
        public string? UUID { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // "Annual Review", "Mid-Year Review"

        [StringLength(500)]
        public string? Description { get; set; } // Optional description for admin context

        public DateTime DateCreated { get; set; }

        [Required]
        [Range(1, 31)]
        public int StartDay { get; set; }

        [Required]
        [Range(1, 12)]
        public int StartMonth { get; set; }

        [Required]
        [Range(1, 31)]
        public int EndDay { get; set; }

        [Required]
        [Range(1, 12)]
        public int EndMonth { get; set; }

        public bool isActive { get; set; } = true;
        public bool isDeleted { get; set; }
        public DateTime? DateDeactivated { get; set; }

        /// <summary>
        /// Converts DTO to Save Model for database operations
        /// </summary>
        public ScoringPeriodSaveModel ToSaveModel(BasicModel basic)
        {
            return new ScoringPeriodSaveModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = this.UUID,
                Name = this.Name,
                Description = this.Description,
                StartDay = this.StartDay,
                StartMonth = this.StartMonth,
                EndDay = this.EndDay,
                EndMonth = this.EndMonth,
                isActive = this.isActive
            };
        }
    }

    /// <summary>
    /// Model for saving Scoring Period data to database
    /// </summary>
    public class ScoringPeriodSaveModel
    {
        public string CompanyUUID { get; set; } = string.Empty;
        public string UsersUUIDLoggedIn { get; set; } = string.Empty;
        public string? UUID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int StartDay { get; set; }
        public int StartMonth { get; set; }
        public int EndDay { get; set; }
        public int EndMonth { get; set; }
        public bool isActive { get; set; }
    }

    /// <summary>
    /// DTO for delete operations response (used in soft delete)
    /// </summary>
    public class ScoringPeriodDeleteResultDto
    {
        public string UUID { get; set; } = string.Empty;
        public bool isValid { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
