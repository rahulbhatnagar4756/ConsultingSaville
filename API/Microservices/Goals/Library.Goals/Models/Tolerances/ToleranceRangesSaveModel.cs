namespace Library.Goals.Models.Tolerances
{
    public class ToleranceRangesSaveModel : IBasicModel
    {
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDLoggedIn { get; set; }
        public string? UUID { get; set; }
        public string? ToleranceSetsUUID { get; set; }
        public decimal? RangeStart { get; set; }
        public decimal Score { get; set; } = 0;

    }
}
