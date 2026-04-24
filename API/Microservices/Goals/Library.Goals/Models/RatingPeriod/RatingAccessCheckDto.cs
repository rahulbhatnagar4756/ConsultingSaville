namespace Library.Goals.Models.RatingPeriod
{
    /// <summary>
    /// DTO for checking if a KPI rating is currently accessible
    /// Used by KPI rating system to validate access
    /// </summary>
    public class RatingAccessCheckDto
    {
        public string RatingPeriodDateUUID { get; set; } = string.Empty;
        public bool IsAccessible { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? DateOpen { get; set; }
        public DateTime? DateClose { get; set; }
        public bool IsActive { get; set; }
    }
}
