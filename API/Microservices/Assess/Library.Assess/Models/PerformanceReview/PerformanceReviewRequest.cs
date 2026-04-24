using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.PerformanceReview
{
    public class PerformanceReviewRequest
    {
        public string Name { get; set; } = string.Empty;
        public string CompanyLogo { get; set; } = string.Empty; // Base64 or URL

        // Optional: Time period for the review
        public string? TimePeriod { get; set; }

        // Quarters scores (Q1, Q2, Q3, Q4, YTD)
        public int[]? Quarters { get; set; }

        // History of positions
        public List<HistoryItem>? History { get; set; }

        // Pillar names (for chart / table)
        public List<string>? Pillars { get; set; }

        // KPI / KPA scores for Page 3
        public List<KpaKpiItem>? KpaKpis { get; set; }

        // **New: Team members for Page 4**
        public List<TeamMember>? TeamMembers { get; set; }
    }

    // New class for team members
    public class TeamMember
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? ImageBase64 { get; set; } = null;
        public string Score { get; set; } = string.Empty;
        public string ScoreColor { get; set; } = string.Empty;
    }
    // Supporting classes
    public class HistoryItem
    {
        public string CreateDate { get; set; } = string.Empty;
        public string BusinessUnit { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Levels { get; set; } = string.Empty;
    }

    public class KpaKpiItem
    {
        public string? Pillar { get; set; } = string.Empty;
        public string? KpaKpi { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Q1 { get; set; } = string.Empty;
        public string? Q2 { get; set; } = string.Empty;
        public string? Q3 { get; set; } = string.Empty;
        public string? Q4 { get; set; } = string.Empty;
    }

}
