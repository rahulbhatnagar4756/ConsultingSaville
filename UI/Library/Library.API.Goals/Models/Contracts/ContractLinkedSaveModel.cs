using Library.API.Goals.Enumerables.Contracts;

namespace Library.API.Goals.Models.Contracts;

public class ContractLinkedSaveModel: ICloneable
{
    public string? ContractPeriodsUUID { get; set; }
    public KPALinkTypes KPALinkTypesid { get; set; }
    public string? LinkedUUID { get; set; }
    public string? KPAUUID { get; set; }  
    public decimal Weight { get; set; } = 100;

    public object Clone()
    {
        return new ContractLinkedSaveModel()
        {
            ContractPeriodsUUID = this.ContractPeriodsUUID,
            KPALinkTypesid = this.KPALinkTypesid,
            LinkedUUID = this.LinkedUUID,
            KPAUUID = this.KPAUUID,
            Weight = this.Weight
        };
    }

    //
}