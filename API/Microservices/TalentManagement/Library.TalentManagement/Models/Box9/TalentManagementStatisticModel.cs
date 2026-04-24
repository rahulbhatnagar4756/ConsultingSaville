using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TalentManagement.Models.Box9
{
    public class TalentManagementStatisticModel
    {
        public int TotalEmployeeStats { get; set; } = 0;
        public int TalentPoolStats { get; set; } = 0;
        public int Box9PlottedStats { get; set; } = 0;
        public int OutStandingStats { get; set; } = 0;

        public int TalentPoolMaleCount { get; set; }
        public int TalentPoolFemaleCount { get; set; }
        public int AvgAgeYears { get; set; }
        public int AvgAgeMonths { get; set; }
        public int AvgExperienceYears { get; set; }
        public int AvgExperienceMonths { get; set; }
        public int SuccessionReadiness { get; set; }
        
        public int MismatchedEmployeeStats { get; set; } = 0;
        public string MismatchedEmployeeLabel { get; set; } = "Mismatched Employee";
        public string? MismatchedEmployeeColor { get; set; } = "#CC6826";
        public int InconsistentPassivePerformerStats { get; set; } = 0;
        public string InconsistentPassivePerformerLabel { get; set; } = "Inconsistent / Passive Performer";
        public string? InconsistentPassivePerformerColor { get; set; } = "#CC6826";
        public int UninspiredJobIncongruenceStats { get; set; } = 0;
        public string UninspiredJobIncongruenceLabel { get; set; } = "Uninspired / Job Incongruence";
        public string? UninspiredJobIncongruenceColor { get; set; } = "#CC6826";
        public int StableEmployeeStats { get; set; } = 0;
        public string StableEmployeeLabel { get; set; } = "Stable Employee";
        public string? StableEmployeeColor { get; set; } = "#7AB3E1";
        public int KeySolidEmployeeStats { get; set; } = 0;
        public string KeySolidEmployeeLabel { get; set; } = "Key / Solid Employee";
        public string? KeySolidEmployeeColor { get; set; } = "#7AB3E1";
        public int DynamicTalentStats { get; set; } = 0;
        public string DynamicTalentLabel { get; set; } = "Dynamic Talent";
        public string? DynamicTalentColor { get; set; } = "#616F85";
        public int TrustedProfessionalStats { get; set; } = 0;
        public string TrustedProfessionalLabel { get; set; } = "Trusted Professional";
        public string? TrustedProfessionalColor { get; set; } = "#7AB3E1";
        public int TalentedFuturePotentialStats { get; set; } = 0;
        public string TalentedFuturePotentialLabel { get; set; } = "Talented / Future Potential";
        public string? TalentedFuturePotentialColor { get; set; } = "#616F85";
        public int TopPerformerStats { get; set; } = 0;
        public string TopPerformerLabel { get; set; } = "Top Performer";
        public string? TopPerformerColor { get; set; } = "#0F1F38";
    }
}
