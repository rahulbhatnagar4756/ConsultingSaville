using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.TalentManagement.Models.SuccessionReadiness;

public class SuccessionReadinessModel
{
    public SuccessionReadinessRolesModel? Role { get; set; }
    public List<SuccessionReadinessRoleUsersCurrentModel>? EmployeeCurrent { get; set; }
    public List<SuccessionReadinessRoleUsersPotentialModel>? EmployeePotential { get; set; }
    public SuccessionReadinessRoleVulnerabilityModel? Vulnerability { get; set; }
}
