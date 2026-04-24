namespace Library.Goals.Models.Contracts
{
    public class ContractPillarModel
    {
        public string? UUID { get; set; }
        public string? ContractPillarsUUID { get; set; }
        public string? Name { get; set; }
        public string? Icons { get; set; }
        public string? IconColor { get; set; }
        public decimal? Weight { get; set; }
        public List<ContractKPAModel>? KPA { get; set; }
    }
}