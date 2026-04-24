using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.Contracts;

public class ContractKPISaveModel
{
    public string? UUID { get; set; }
    public string KPAUUID { get; set; } = string.Empty;
    public string UsersUUID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? StatusUUID { get; set; }
    public string? ToleranceSetsUUID { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public decimal Target { get; set; } = 100;
    public decimal Weights { get; set; } = 100;
    public string? TrackingUUID { get; set; }
    public bool isScoreProcessing { get; set; } = true;

    // For basic model inheritance
    public string CompanyUUID { get; set; } = string.Empty;
    public string UsersUUIDLoggedIn { get; set; } = string.Empty;
}
