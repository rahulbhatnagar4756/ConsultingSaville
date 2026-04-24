using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.TalentFitReport
{
    /// <summary>
    /// Represents the request data to generate a Talent Report PDF.
    /// </summary>
    public class TalentFitReport_Request
    {
        /// <summary>
        /// Name of the company.
        /// </summary>
        [Required]
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>
        /// Company logo in Base64 format or URL.
        /// </summary>
        [Required]
        public string CompanyLogo { get; set; } = string.Empty; // Base64 or URL

        /// <summary>
        /// Name of the entity powering the report.
        /// </summary>

        [Required]
        public string PoweredByLogo { get; set; } = string.Empty;

        /// <summary>
        /// Name of the employee for whom the report is generated.
        /// </summary>
        [Required]
        public string EmployeeName { get; set; } = string.Empty;

        /// <summary>
        /// Date of report generation.
        /// </summary>
        [Required]
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// Employee position or designation.
        /// </summary>

        [Required]
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// Employee role.
        /// </summary>

        [Required]
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Type of report (e.g., "Confidential").
        /// </summary>
        [Required]
        public string ReportType { get; set; } = string.Empty; // e.g., "Confidential"

        /// <summary>
        /// Footer text to be displayed in the report.
        /// </summary>
        [Required]
        public string FooterText { get; set; }

        // Footer content    
        [Required]
        public string FooterUrl { get; set; }

        /// <summary>
        /// Content for page 1.
        /// </summary>
        [Required]
        public TalentFitReportPage1 Page1 { get; set; } = new TalentFitReportPage1();
        /// <summary>
        /// Content for page 4.
        /// </summary>
        [Required]
        public TalentFitReportPage4 Page4 { get; set; } = new TalentFitReportPage4();

        /// <summary>
        /// Content for page 5.
        /// </summary>
        [Required]
        public TalentReportPage5 Page5 { get; set; } = new TalentReportPage5();       

        [Required]
        public TalentReportLastPage LastPage { get; set; } = new();
    }

    public class QA
    {
        public InterviewSection InterviewSection { get; set; }
    }

    public class InterviewQuestion
    {
        public string Question { get; set; }
        public List<string> SubQuestions { get; set; }
    }

    public class InterviewSection
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<InterviewQuestion> Questions { get; set; }
    }

    public class TalentFitReport_SelectionRequest : TalentFitReport_Request
    {
        [Required]
        public TalentReportDevelopmentPage6 Page6 { get; set; } = new();
      
    }

    public class TalentFitReport_InterviewRequest : TalentFitReport_Request
    {
        /// <summary>
        /// Content for page 7.
        /// </summary>
        [Required]
        public QA Page7 { get; set; } = new QA();
        /// <summary>
        /// Content for page 8.
        /// </summary>
        [Required]
        public QA Page8 { get; set; } = new QA();
        /// <summary>
        /// Content for page 9.
        /// </summary>
        [Required]
        public QA Page9 { get; set; } = new QA();
        /// <summary>
        /// Content for page 10.
        /// </summary>
        [Required]
        public QA Page10 { get; set; } = new QA();
        /// <summary>
        /// Content for page 11.
        /// </summary>
        [Required]
        public QA Page11 { get; set; } = new QA();
        /// <summary>
        /// Content for page 12.
        /// </summary>
        [Required]
        public QA Page12 { get; set; } = new QA();
        /// <summary>
        /// Content for page 13.
        /// </summary>
        [Required]
        public QA Page13 { get; set; } = new QA();
        /// <summary>
        /// Content for page 14.
        /// </summary>
        [Required]
        public QA Page14 { get; set; } = new QA();
        /// <summary>
        /// Content for page 15.
        /// </summary>
        [Required]
        public QA Page15 { get; set; } = new QA();

        /// <summary>
        /// Content for page 16.
        /// </summary>
        [Required]
        public TalentFitReportPage16 Page16 { get; set; } = new TalentFitReportPage16();
    }

    public class TalentFitReport_DevelopmentRequest : TalentFitReport_Request
    {
        [Required]
        public TalentReportSelectionPage6 Page6 { get; set; } = new();
        [Required]
        public TalentReportSelectionPage7 Page7 { get; set; } = new();

        [Required]
        public TalentReportSelectionPage8 Page8 { get; set; } = new();
    }

    /// <summary>
    /// Page 2 content of the Personality Report.
    /// </summary>
    public class TalentReportSelectionPage7
    {
        [Required]
        public List<string> PerformanceEnhancers { get; set; }
        [Required]
        public List<string> PerformanceInhibitors { get; set; }
    }

    /// <summary>
    /// Page 3 content of the Personality Report.
    /// </summary>
    public class TalentReportSelectionPage8
    {
        public string TeamType { get; set; }
        public GridPositions GridPositions { get; set; }
        public List<string> ThoughtsAboutMyself { get; set; }
        public List<string> MyFrustrations { get; set; }
        public List<string> OthersThoughtsAboutMe { get; set; }
        public List<string> WhoComplementsMe { get; set; }
        public List<string> Teamwork { get; set; }
        public List<string> Leadership { get; set; }
        public List<string> HowIManage { get; set; }
        public List<string> PerformingAtMyBest { get; set; }
    }

    public class GridPositions
    {
        public TeamGridBoxes WarmTeam { get; set; }
        public TeamGridBoxes CoolTeam { get; set; }
    }

    // Class to hold name and color
    public class GridBox
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }

    // Updated TeamGridBoxes class
    public class TeamGridBoxes
    {
        public GridBox TopLeft { get; set; }
        public GridBox TopRight { get; set; }
        public GridBox BottomLeft { get; set; }
        public GridBox BottomRight { get; set; }
    }

    public class GridPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Intensity { get; set; } // "light", "medium", "dark"
    }


    /// <summary>
    /// Page 1 content of the Talent Report.
    /// </summary>
    public class TalentFitReportPage1
    {
        [Required]
        public string ReportTitle { get; set; } = string.Empty;

        [Required]
        public string ReportSubtitle { get; set; } = string.Empty;
    }

    /// <summary>
    /// Page 4 content of the Talent Report.
    /// </summary>
    public class TalentFitReportPage4
    {
        [Required]
        public DateTime ReportDate { get; set; }

        // Page title
        [Required]
        public string PageTitle { get; set; } = string.Empty; // "INTRODUCTION"

        // Summary section title
        [Required]
        public string SummaryProfileTitle { get; set; } = string.Empty; // "Summary Profile"

        // NEW TALENT MATCH FIELDS
        [Required]
        public decimal FitForThisRole { get; set; } // 7.1
        [Required]
        public int CultureFitScore { get; set; } // 7
        [Required]
        public string CultureFitStyles { get; set; } // "Fairly High"

        // Four quadrants - titles and content
        [Required]
        public string KeyStrengthTitle { get; set; } = string.Empty;
        public List<string> KeyStrengthContent { get; set; } = new List<string>();

        [Required]
        public string DevelopmentNeedsTitle { get; set; } = string.Empty;
        public List<string> DevelopmentNeedsContent { get; set; } = new List<string>();

        [Required]
        public string PotentialRisksTitle { get; set; } = string.Empty;
        public List<string> PotentialRisksContent { get; set; } = new List<string>();

        [Required]
        public string UntappedPotentialTitle { get; set; } = string.Empty;
        public List<string> UntappedPotentialContent { get; set; } = new List<string>();
        //Data Parameters
        [Required]
        public string  KeyStrengthData { get; set; }
        [Required]
        public string DevelopmentNeedsData { get; set; }
        [Required]
        public string PotentialRisksData { get; set; }
        [Required]
        public string UntappedPotentialData { get; set; }
    }

    /// <summary>
    /// Page 5 content of the Talent Report.
    /// </summary>
    public class TalentReportPage5
    {
        public string ProfileName { get; set; }
        public string Section1Name { get; set; }
        public List<ProfileItem> Section1 { get; set; }
        public string Section2Name { get; set; }
        public List<ProfileItem> Section2 { get; set; }
    }
    /// <summary>
    /// Page 7 content of the Talent Report.
    /// </summary>
    public class TalentReportLastPage
    {
        public string? JobAnalysis { get; set; }
        public string? JobAnalysisDate { get; set; }

        public List<AssessmentMethod>? Assessments { get; set; }

        // --- NEW FIELDS FOR RESPONSE QUALITY INDICATORS ---
        public List<AbilityScore>? AbilityScores { get; set; }
        public List<PersonalityScore>? PersonalityScores { get; set; }

        public string? InputData { get; set; }
        public string? TemplateVersion { get; set; }
    }

    public class AbilityScore
    {
        public string? AbilityName { get; set; }   // Verbal, Numerical, Diagrammatic
        public int Percentile { get; set; }
        public int Pace { get; set; }
        public int MarkerPosition { get; set; }   // 1 - 10
    }

    public class PersonalityScore
    {
        public string? TraitName { get; set; }   // Consistency of Ranking
        public int Score { get; set; }
        public string? Description { get; set; }
    }



    /// <summary>
    /// Page 6 content of the Talent Report.
    /// </summary>
    public class TalentReportDevelopmentPage6
    {
        public List<DevelopmentSection> DevelopmentSections { get; set; } = new List<DevelopmentSection>();
    }

    /// <summary>
    /// Assessment method in the report.
    /// </summary>
    public class AssessmentMethod
    {
        public string? TestName { get; set; }
        public string? Norm { get; set; }
        public string? Date { get; set; }
    }

    /// <summary>
    /// Development section in the report.
    /// </summary>
    public class DevelopmentSection
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Tips { get; set; } = new List<string>();
    }

    /// <summary>
    /// Individual profile item.
    /// </summary>
    public class ProfileItem
    {
        public string Name { get; set; }
        public int Score { get; set; } // 1-10
    }

    /// <summary>
    /// Page 6 content of the Talent Report.
    /// </summary>
    public class TalentReportSelectionPage6
    {
        public List<BehaviouralItem> ProblemSolving { get; set; } = new List<BehaviouralItem>();
        public List<BehaviouralItem> InfluencingPeople { get; set; } = new List<BehaviouralItem>();
        public List<BehaviouralItem> AdaptingApproaches { get; set; } = new List<BehaviouralItem>();
        public List<BehaviouralItem> DeliveringSuccess { get; set; } = new List<BehaviouralItem>();
    }


    /// <summary>
    /// Behavioural item with sub-behaviours.
    /// </summary>
    public class BehaviouralItem
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public List<ProfileItemPage6> SubBehaviors { get; set; } = new List<ProfileItemPage6>();
    }

    /// <summary>
    /// Individual profile item.
    /// </summary>
    public class ProfileItemPage6
    {
        public string Name { get; set; }
        public int Score { get; set; } // 1-10
    }

    public class TalentFitReportPage16
    {
        [Required]
        public string InterviewerName { get; set; } = string.Empty;

        [Required]
        public string InterviewDate { get; set; } = string.Empty;
        [Required]
        public string RoleAppliedFor { get; set; } = string.Empty;
    }

    public class TalentFitReport_Response
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public byte[]? PdfData { get; set; }
        public string? FileName { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
