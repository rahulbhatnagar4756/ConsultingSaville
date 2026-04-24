using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.TalentManagement.Models
{
    public class TalentManagementStatisticModel:ICloneable
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

        public object Clone()
        {
            return new TalentManagementStatisticModel
            {
                TotalEmployeeStats = this.TotalEmployeeStats,
                TalentPoolStats = this.TalentPoolStats,
                Box9PlottedStats = this.Box9PlottedStats,
                OutStandingStats = this.OutStandingStats,
                TalentPoolMaleCount = this.TalentPoolMaleCount,
                TalentPoolFemaleCount = this.TalentPoolFemaleCount,
                AvgAgeYears = this.AvgAgeYears,
                AvgAgeMonths = this.AvgAgeMonths,
                AvgExperienceYears = this.AvgExperienceYears,
                AvgExperienceMonths = this.AvgExperienceMonths,
                SuccessionReadiness = this.SuccessionReadiness,
                MismatchedEmployeeStats = this.MismatchedEmployeeStats,
                MismatchedEmployeeLabel = this.MismatchedEmployeeLabel,
                MismatchedEmployeeColor = this.MismatchedEmployeeColor,
                InconsistentPassivePerformerStats = this.InconsistentPassivePerformerStats,
                InconsistentPassivePerformerLabel = this.InconsistentPassivePerformerLabel,
                InconsistentPassivePerformerColor = this.InconsistentPassivePerformerColor,
                UninspiredJobIncongruenceStats = this.UninspiredJobIncongruenceStats,
                UninspiredJobIncongruenceLabel = this.UninspiredJobIncongruenceLabel,
                UninspiredJobIncongruenceColor = this.UninspiredJobIncongruenceColor,
                StableEmployeeStats = this.StableEmployeeStats,
                StableEmployeeLabel = this.StableEmployeeLabel,
                StableEmployeeColor = this.StableEmployeeColor,
                KeySolidEmployeeStats = this.KeySolidEmployeeStats,
                KeySolidEmployeeLabel = this.KeySolidEmployeeLabel,
                KeySolidEmployeeColor = this.KeySolidEmployeeColor,
                DynamicTalentStats = this.DynamicTalentStats,
                DynamicTalentLabel = this.DynamicTalentLabel,
                DynamicTalentColor = this.DynamicTalentColor,
                TrustedProfessionalStats = this.TrustedProfessionalStats,
                TrustedProfessionalLabel = this.TrustedProfessionalLabel,
                TrustedProfessionalColor = this.TrustedProfessionalColor,
                TalentedFuturePotentialStats = this.TalentedFuturePotentialStats,
                TalentedFuturePotentialLabel = this.TalentedFuturePotentialLabel,
                TalentedFuturePotentialColor = this.TalentedFuturePotentialColor,
                TopPerformerStats = this.TopPerformerStats,
                TopPerformerLabel = this.TopPerformerLabel,
                TopPerformerColor = this.TopPerformerColor
                            };
        }
    }


}
