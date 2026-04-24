namespace Library.Goals.Models.RatingPeriod
{
    /// <summary>
    /// Model for saving Rating Period data to database
    /// </summary>
    public class RatingPeriodSaveModel
    {
        public string CompanyUUID { get; set; } = string.Empty;
        public string UsersUUIDLoggedIn { get; set; } = string.Empty;
        public string? UUID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
