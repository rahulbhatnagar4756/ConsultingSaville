namespace Library.Goals.Models.Contracts
{
    public class ContractKPIModel
    {
        public string? UUID { get; set; }
        public string? KPAUUID { get; set; }
        public string? KPAKPIUUID { get; set; }
        public string? CompanyUUID { get; set; }
        public string? UsersUUID { get; set; }
        public string? StatusUUID { get; set; }
        public string? Status { get; set; }
        public string? RatingPeriodsUUID { get; set; }
        public string? RatingPeriods { get; set; }
        public string? RatingPeriodsAbs { get; set; }
        public string? ToleranceSetsUUID { get; set; }
        public string? ToleranceSets { get; set; }
        public string? ToleranceSetsDescription { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public decimal? Target { get; set; }
        public decimal? Weight { get; set; }
        public bool isScoreProcessing { get; set; }
        public bool isActive { get; set; }
    }
}