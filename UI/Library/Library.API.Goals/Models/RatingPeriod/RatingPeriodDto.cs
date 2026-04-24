using System.ComponentModel.DataAnnotations;

namespace Library.API.Goals.Models.RatingPeriod
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
    }
}
