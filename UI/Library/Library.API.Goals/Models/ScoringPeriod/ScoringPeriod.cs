using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.ScoringPeriod
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
    }

    /// <summary>
    /// DTO for delete operations response
    /// </summary>
    public class DeleteResultDto
    {
        public string UUID { get; set; } = string.Empty;
        public bool isValid { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
