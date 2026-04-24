using System.ComponentModel.DataAnnotations;

namespace Library.Assess.Models.TalentMatchReportPDF
{

    /// <summary>
    /// Represents the request data to generate a Talent Report PDF.
    /// </summary>
    public class TalentMatchReportRequest
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
        public string PoweredByName { get; set; } = string.Empty;
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
        /// Type of report (e.g., "Confidential").
        /// </summary>
        [Required]
        public string ReportType { get; set; } = string.Empty; // e.g., "Confidential"

        /// <summary>
        /// Footer text to be displayed in the report.
        /// </summary>
        [Required]
        public string FooterText { get; set; }

        /// <summary>
        /// Content for page 1.
        /// </summary>
        [Required]
        public TalentReportPage1 Page1 { get; set; } = new TalentReportPage1();

        /// <summary>
        /// Content for page 2.
        /// </summary>
        [Required]
        public TalentReportPage2 Page2 { get; set; } = new TalentReportPage2();

        /// <summary>
        /// Content for page 3.
        /// </summary>
        [Required]
        public TalentReportPage3 Page3 { get; set; } = new TalentReportPage3();

        /// <summary>
        /// Content for page 4.
        /// </summary>
        [Required]
        public TalentReportPage4 Page4 { get; set; } = new TalentReportPage4();

        /// <summary>
        /// Content for page 7.
        /// </summary>
        [Required]
        public TalentReportPage7 Page7 { get; set; } = new TalentReportPage7();
    }

    /// <summary>
    /// Page 1 content of the Talent Report.
    /// </summary>
    public class TalentReportPage1
    {
        [Required]
        public string ReportTitle { get; set; } = string.Empty;

        [Required]
        public string ReportSubtitle { get; set; } = string.Empty;

        [Required]
        public string DevelopmentSummary { get; set; } = string.Empty;

        [Required]
        public string UsageInstructions { get; set; } = string.Empty;

        [Required]
        public string ValidityPeriod { get; set; } = string.Empty;

    }

    /// <summary>
    /// Page 2 content of the Talent Report.
    /// </summary>
    public class TalentReportPage2
    {
        [Required]
        public DateTime ReportDate { get; set; }

        // Page title
        [Required]
        public string PageTitle { get; set; } = string.Empty; // "INTRODUCTION"

        // Introduction paragraphs - all dynamic
        [Required]
        public string IntroductionParagraph2 { get; set; } = string.Empty;

        // Summary section title
        [Required]
        public string SummaryProfileTitle { get; set; } = string.Empty; // "Summary Profile"

        // NEW TALENT MATCH FIELDS
        [Required]
        public decimal FitForThisRole { get; set; } // 7.1
        [Required]
        public int BehaviouralStylesScore { get; set; } // 7
        [Required]
        public string BehaviouralStyles { get; set; } // "Fairly High"
        [Required]
        public int AbilitiesandSkillsScore { get; set; } // 7
        [Required]
        public string AbilitiesandSkills { get; set; } // "Fairly High"

        // Four quadrants - titles and content
        [Required]
        public string PotentialLimitationsTitle { get; set; } = string.Empty;
        public List<string> PotentialLimitationsContent { get; set; } = new List<string>();

        [Required]
        public string KeyStrengthsTitle { get; set; } = string.Empty;
        public List<string> KeyStrengthsContent { get; set; } = new List<string>();

        [Required]
        public string DevelopmentOpportunitiesTitle { get; set; } = string.Empty;
        public List<string> DevelopmentOpportunitiesContent { get; set; } = new List<string>();

        [Required]
        public string GoodPotentialTitle { get; set; } = string.Empty;
        public List<string> GoodPotentialContent { get; set; } = new List<string>();

        // Colors for quadrants
        [Required]
        public string PotentialLimitationsColor { get; set; }
        [Required]
        public string KeyStrengthsColor { get; set; }
        [Required]
        public string DevelopmentOpportunitiesColor { get; set; }
        [Required]
        public string GoodPotentialColor { get; set; }

        // Footer content    
        [Required]
        public string FooterUrl { get; set; }
    }

    /// <summary>
    /// Page 3 content of the Talent Report.
    /// </summary>
    public class TalentReportPage3
    {
        public List<ProfileItem> EssentialBehaviours { get; set; }
        public List<ProfileItem> ImportantBehaviours { get; set; }
        public List<ProfileItem> EssentialSkills { get; set; }
    }

    /// <summary>
    /// Page 4 content of the Talent Report.
    /// </summary>
    public class TalentReportPage4
    {
        public List<BehaviouralItem> ProblemSolving { get; set; } = new List<BehaviouralItem>();
        public List<BehaviouralItem> InfluencingPeople { get; set; } = new List<BehaviouralItem>();
        public List<BehaviouralItem> AdaptingApproaches { get; set; } = new List<BehaviouralItem>();
        public List<BehaviouralItem> DeliveringSuccess { get; set; } = new List<BehaviouralItem>();
    }

    /// <summary>
    /// Page 7 content of the Talent Report.
    /// </summary>
    public class TalentReportPage7
    {
        public string? JobAnalysis { get; set; }
        public string? JobAnalysisDate { get; set; }

        public List<AssessmentMethod>? Assessments { get; set; }

        public string? InputData { get; set; }
        public string? TemplateVersion { get; set; }
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
    /// Behavioural item with sub-behaviours.
    /// </summary>
    public class BehaviouralItem
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public List<ProfileItem> SubBehaviors { get; set; } = new List<ProfileItem>();
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
    /// Response returned after generating a Talent Report.
    /// </summary>
    public class TalentMatchReportResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public byte[]? PdfData { get; set; }
        public string? FileName { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
