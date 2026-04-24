namespace Library.Goals.Models.Contracts
{
    public class ContractEnterpriseStructureModel
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? Icons { get; set; }
        public string? IconColor { get; set; }
        public List<ContractPillarModel>? Pillars { get; set; }
    }
}