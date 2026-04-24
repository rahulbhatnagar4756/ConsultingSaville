using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Assess.Models.Projects;

public class CandidateTestModel
{
    public string UUID { get; set; } = string.Empty;
    public string CompanyUUID { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string UsersUUID { get; set; } = string.Empty;
    public string ProjectsUUID { get; set; } = string.Empty;
    public string Projects { get; set; } = string.Empty;
    public DateTime? TimeAllowedStart { get; set; }
    public DateTime? TimeAllowedEnd { get; set; }
    public int TestsUUID { get; set; }
    public string TestName { get; set; } = string.Empty;
    public string? Instructure { get; set; }
    public string? Header { get; set; }
    public int? AverageTimeTakenInMinutes { get; set; }
    public bool isTimed { get; set; }
    public int? TimeLimitMinutes { get; set; }
    public string? Icon { get; set; }
    public string? ImageURL { get; set; }
    public bool isAllowMobileUI { get; set; }


    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public string StatusUUID { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public string? ExternalKey { get; set; }
    public string? URLAssessmentLink { get; set; }

    // Helper properties for UI
    public string TestImage => !string.IsNullOrEmpty(ImageURL) ? ImageURL : Icon ?? "quiz";
    public bool IsImageUrl => !string.IsNullOrEmpty(ImageURL);
    public string TimeLimit => isTimed && TimeLimitMinutes.HasValue
        ? $"{TimeLimitMinutes} minutes"
        : "No time limit";
    public string AverageTime => AverageTimeTakenInMinutes.HasValue
        ? $"~{AverageTimeTakenInMinutes} min avg"
        : "";
}
