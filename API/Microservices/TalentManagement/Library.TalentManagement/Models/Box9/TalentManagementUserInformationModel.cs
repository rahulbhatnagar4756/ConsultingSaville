using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TalentManagement.Models.Box9;

public class TalentManagementUserInformationModel
{
    public string? UsersUUID { get; set; }
    public string? FrogsUUID { get; set; }
    public string? FrogSetsUUID { get; set; }
    public string? FrogElementsUUID { get; set; }
    public string? FrogElementsUUIDNextRole { get; set; }
    public string? SuccessionPlanningStatus { get; set; }
    public string? SuccessionPlanningStatusURL { get; set; }
    public string? IDPStatus { get; set; }
    public string? IDPStatusURL { get; set; }
    public double? TenureInCompany { get; set; }
    public double? TenureInCurrentRole { get; set; }
    public string? PotentialRating { get; set; }
    public string? Qualification { get; set; }
    public string? ReadinessForNextRoleManager { get; set; }
    public string? ReadinessForNextRoleHR { get; set; }
    public string? ReadinessForNextRole { get; set; }
    public int? FrogLookupTypesidReadinessForNextRole { get; set; }
    public int? FrogLookupTypesidNextRole { get; set; }
    public string? WhatIsTheNextRole { get; set; }
    public string? RequirementsForNextRole { get; set; }
    public string? GenericUUID { get; set; }
}
