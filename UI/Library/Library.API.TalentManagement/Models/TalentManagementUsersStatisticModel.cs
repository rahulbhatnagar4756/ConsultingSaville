namespace Library.API.TalentManagement.Models
{
    public class TalentManagementUsersStatisticModel
    {

        public int? FinalGoalScore { get; set; } = 92;
        public string? CPPMatch { get; set; } = "EP";
        public string? PersonalityMatch { get; set; } = "UM";
        public int Box9Types { get; set; } = 3;
        public string? Box9TypeActions { get; set; }

        public string MismatchedEmployeeLabel { get; set; } = "Mismatched Employee";
        public string? MismatchedEmployeeColor { get; set; } = "#CC6826";
        public string InconsistentPassivePerformerLabel { get; set; } = "Inconsistent / Passive Performer";
        public string? InconsistentPassivePerformerColor { get; set; } = "#CC6826";
        public string UninspiredJobIncongruenceLabel { get; set; } = "Uninspired / Job Incongruence";
        public string? UninspiredJobIncongruenceColor { get; set; } = "#CC6826";
        public string StableEmployeeLabel { get; set; } = "Stable Employee";
        public string? StableEmployeeColor { get; set; } = "#7AB3E1";
        public string KeySolidEmployeeLabel { get; set; } = "Key / Solid Employee";
        public string? KeySolidEmployeeColor { get; set; } = "#7AB3E1";
        public string DynamicTalentLabel { get; set; } = "Dynamic Talent";
        public string? DynamicTalentColor { get; set; } = "#616F85";
        public string TrustedProfessionalLabel { get; set; } = "Trusted Professional";
        public string? TrustedProfessionalColor { get; set; } = "#7AB3E1";
        public string TalentedFuturePotentialLabel { get; set; } = "Talented / Future Potential";
        public string? TalentedFuturePotentialColor { get; set; } = "#616F85";
        public string TopPerformerLabel { get; set; } = "Top Performer";
        public string? TopPerformerColor { get; set; } = "#0F1F38";


        public override string ToString()
        {
            if (FinalGoalScore == null && CPPMatch == null && PersonalityMatch == null)  return "";
            return $"Performance Score: {FinalGoalScore} <br/>CPP Match: {CPPMatch} <br/>Personality Match: {PersonalityMatch}";
        }

    }


}
