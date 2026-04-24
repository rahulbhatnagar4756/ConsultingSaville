namespace Library.Goals.Models.RatingPeriod
{
    /// <summary>
    /// Model for saving Rating Period Date data to database
    /// </summary>
    public class RatingPeriodDateSaveModel
    {
        public string CompanyUUID { get; set; } = string.Empty;
        public string UsersUUIDLoggedIn { get; set; } = string.Empty;
        public string? UUID { get; set; }
        public string RatingPeriodsUUID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime DateOpen { get; set; }
        public DateTime? DateClose { get; set; }
        public bool isActive { get; set; } = true;
    }
}
