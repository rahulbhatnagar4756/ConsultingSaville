using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.SelectionStrategyReport
{
    public class SelectionStrategyReportRequest
    {
        public string CompanyLogo { get; set; }
        public string ReportTitle { get; set; }
        public string PoweredBy { get; set; }
        public string PoweredByLogo { get; set; }
        public string Candidate  { get; set; }
        public string Project  { get; set; }
        public string DateReport { get; set; }
        public string JobAnalysis { get; set; }
        public string DateOfAnalysis { get; set; }
        public SelectionStrategyData SelectionStrategy { get; set; }
    }

    /// <summary>
    /// Response returned after generating a Talent Report.
    /// </summary>
    public class SelectionStrategyReportResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public byte[]? PdfData { get; set; }
        public string? FileName { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class SelectionStrategyData
    {
        public List<CandidateScore> Candidates { get; set; }
        public List<CompetencyColumn> Competencies { get; set; }
        public List<AbilityColumn> Abilities { get; set; }
        public ScoringLegend Legend { get; set; }
    }

    public class CandidateScore
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<ScoreRating> CompetencyScores { get; set; }
        public List<ScoreRating> AbilityScores { get; set; }
        public string Investment { get; set; } // "H", "M", "L"
        public int Ranking { get; set; }
        public string Status { get; set; } // "EH" (Employable - Hire)
    }

    public class ScoreRating
    {
        public string CompetencyOrAbility { get; set; }
        public int Rating { get; set; } // "E" (Exceptional), "G" (Good), "P" (Potential), "M" (Marginal), "U" (Unsuitable)
        public string ColorCode { get; set; } // For visual representation
    }

    public class CompetencyColumn
    {
        public string Name { get; set; }
        public int Order { get; set; }
    }

    public class AbilityColumn
    {
        public string Name { get; set; }
        public int Order { get; set; }
    }

    public class ScoringLegend
    {
        public List<RatingDefinition> Ratings { get; set; }
        public List<InvestmentDefinition> Investments { get; set; }
    }

    public class RatingDefinition
    {
        public string Rating { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
    }

    public class InvestmentDefinition
    {
        public string Level { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
    }
}
