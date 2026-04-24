using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.Contracts;

public class ContractsModel
{
    /// <summary>
    /// Unique identifier for the contract.
    /// </summary>
    public string? ContractsUUID { get; set; }

    /// <summary>
    /// Unique identifier for the associated company.
    /// </summary>
    public string? CompanyUUID { get; set; }

    /// <summary>
    /// Name of the company associated with the contract.
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// Unique identifier for the user associated with the contract.
    /// </summary>
    public string? UsersUUID { get; set; }

    /// <summary>
    /// Unique identifier for the template associated with the contract.
    /// </summary>
    public string? TemplatesUUID { get; set; }

    /// <summary>
    /// Name of the contract owner or responsible person.
    /// </summary>
    public string? ContractOwnerName { get; set; }

    /// <summary>
    /// Unique identifier for the contract period.
    /// </summary>
    public string? ContractPeriodsUUID { get; set; }

    /// <summary>
    /// Display name or description of the contract period.
    /// </summary>
    public string? ContractPeriods { get; set; }

    /// <summary>
    /// The start date of the contract.
    /// </summary>
    public DateTime? DateStart { get; set; }

    /// <summary>
    /// The end date of the contract.
    /// </summary>
    public DateTime? DateEnd { get; set; }

    /// <summary>
    /// Indicates whether the current user is linked to the contract.
    /// </summary>
    public bool isUser { get; set; }

    /// <summary>
    /// Indicates whether this contract is a template.
    /// </summary>
    public bool isTemplate { get; set; }

    /// <summary>
    /// Indicates whether the contract is currently active.
    /// </summary>
    public bool isActive { get; set; }

    /// <summary>
    /// Indicates whether the contract is marked as deleted.
    /// </summary>
    public bool isDeleted { get; set; }

    /// <summary>
    /// Enterprise structures associated with the contract.
    /// </summary>
    public List<ContractEnterpriseStructureModel>? EnterpriseStructures { get; set; }
}

public class ContractEnterpriseStructureModel
{
    public string? UUID { get; set; }
    public string? Name { get; set; }
    public string? Icons { get; set; }
    public string? IconColor { get; set; }
    public List<ContractPillarModel>? Pillars { get; set; }
}

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

public class ContractKPAModel
{
    public string? UUID { get; set; }
    public string? ContractPillarsUUID { get; set; }
    public string? StatusUUID { get; set; }
    public string? Status { get; set; }
    public string? RatingPeriodsUUID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Weight { get; set; }
    public bool isActive { get; set; }
    public bool isSelected { get; set; } = false;
    public List<ContractKPIModel>? KPI { get; set; }
    public List<ContractKPALinkedModel>? Linked { get; set; }
}

public class ContractKPIModel
{
    public string? UUID { get; set; }
    public string? KPAUUID { get; set; }
    public string? KPAKPIUUID { get; set; }
    public string? CompanyUUID { get; set; }
    public string? UsersUUID { get; set; }
    public string? StatusUUID { get; set; }
    public string? Status { get; set; }
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
    public bool isSelected { get; set; } = false;

    }

public class ContractKPALinkedModel
{
    public string? UUID { get; set; }
    public string? KPALinkUUID { get; set; }
    public string? CompanyUUID { get; set; }
    public string? KPALinkTypes { get; set; } // "KPI" or "KPA"
    public string? KPALinkTypesid { get; set; } // "KPI" or "KPA"
    public string? UsersUUID { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Fullname => Firstname + " " + Lastname;
    public string? Email { get; set; }
    public string? IDNumber { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Weight { get; set; }
}