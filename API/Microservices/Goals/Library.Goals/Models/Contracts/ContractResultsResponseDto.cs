using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.Contracts
{
    /// <summary>
    /// Represents the root response from the contract results stored procedure.
    /// </summary>
    public class ContractResultsResponseDto
    {
        /// <summary>
        /// Gets or sets the collection of contract results data.
        /// </summary>
        public List<ContractResultsDto>? Data { get; set; }
    }

    /// <summary>
    /// Represents the complete contract results with all nested structures.
    /// </summary>
    public class ContractResultsDto
    {
        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the contract.
        /// </summary>
        public string? ContractsUUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the company.
        /// </summary>
        public string? CompanyUUID { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        public string? Company { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the user.
        /// </summary>
        public string? UsersUUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the template.
        /// </summary>
        public string? TemplatesUUID { get; set; }

        /// <summary>
        /// Gets or sets the name of the contract owner (user or template name).
        /// </summary>
        public string? ContractOwnerName { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the contract period.
        /// </summary>
        public string? ContractPeriodsUUID { get; set; }

        /// <summary>
        /// Gets or sets the name of the contract period.
        /// </summary>
        public string? ContractPeriods { get; set; }

        /// <summary>
        /// Gets or sets the start date of the contract period.
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// Gets or sets the end date of the contract period.
        /// </summary>
        public DateTime DateEnd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user logged in is manager or admin of goals.
        /// </summary>
        public bool IsManagerOrAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contract is for a user (1) or not (0).
        /// </summary>
        public int IsUser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contract is for a template (1) or not (0).
        /// </summary>
        public int IsTemplate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contract is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contract is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the employee details for the logged-in user.
        /// </summary>
        public EmployeeDetailsDto? EmployeeDetails { get; set; }

        /// <summary>
        /// Gets or sets the collection of enterprise structures with their pillars.
        /// </summary>
        public List<EnterpriseStructureDto>? EnterpriseStructures { get; set; }
    }

    public class EmployeeDetailsDto
    {
        /// <summary>
        /// Gets or sets the user's unique identifier.
        /// </summary>
        public string? UsersUUID { get; set; }

        /// <summary>
        /// Gets or sets the company's unique identifier.
        /// </summary>
        public string? CompanyUUID { get; set; }

        /// <summary>
        /// Gets or sets the employee's first name.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the employee's middle name.
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the employee's last name.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the employee's email address.
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Gets or sets the employee's ID Number.
        /// </summary>
        public string? IDNumber { get; set; }
        /// <summary>
        /// Gets or sets the employee's Business Units.
        /// </summary>
        public string? EmployeeBusinessUnits { get; set; }
        /// <summary>
        /// Gets or sets the employee's Departments.
        /// </summary>
        public string? EmployeeDepartments { get; set; }
        /// <summary>
        /// Gets or sets the employee's Position.
        /// </summary>
        public string? EmployeeJobs { get; set; }

        /// <summary>
        /// Gets or sets the employee's mobile number.
        /// </summary>
        public string? Mobile { get; set; }

        /// <summary>
        /// Gets or sets the employee number.
        /// </summary>
        public string? EmployeeNumber { get; set; }

        /// <summary>
        /// Gets or sets the employee's level name.
        /// </summary>
        public string? EmployeeLevels { get; set; }

        /// <summary>
        /// Gets or sets the manager's full name.
        /// </summary>
        public string? FullnameManager { get; set; }

        /// <summary>
        /// Gets or sets the manager's email address.
        /// </summary>
        public string? EmailManager { get; set; }

        /// <summary>
        /// Gets or sets the manager's ID number.
        /// </summary>
        public string? IDNumberManager { get; set; }

        /// <summary>
        /// Gets or sets the employee's date of birth.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the employee's age.
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the employee has an image.
        /// </summary>
        public bool? IsImage { get; set; }

        /// <summary>
        /// Gets or sets the employee's image (Base64 or URL).
        /// </summary>
        public string? Image { get; set; }
    }


    /// <summary>
    /// Represents an enterprise structure type with its associated pillars.
    /// </summary>
    public class EnterpriseStructureDto
    {
        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the enterprise structure type.
        /// </summary>
        public string? UUID { get; set; }

        /// <summary>
        /// Gets or sets the name of the enterprise structure type.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the icon(s) associated with the enterprise structure type.
        /// </summary>
        public string? Icons { get; set; }

        /// <summary>
        /// Gets or sets the icon color for the enterprise structure type.
        /// </summary>
        public string? IconColor { get; set; }

        /// <summary>
        /// Gets or sets the collection of pillars within this enterprise structure.
        /// </summary>
        public List<PillarDto>? Pillars { get; set; }
    }

    /// <summary>
    /// Represents a pillar within an enterprise structure.
    /// </summary>
    public class PillarDto
    {
        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the pillar.
        /// </summary>
        public string? UUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the contract pillar.
        /// </summary>
        public string? ContractPillarsUUID { get; set; }

        /// <summary>
        /// Gets or sets the name of the pillar.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the icon(s) associated with the pillar.
        /// </summary>
        public string? Icons { get; set; }

        /// <summary>
        /// Gets or sets the icon color for the pillar.
        /// </summary>
        public string? IconColor { get; set; }

        /// <summary>
        /// Gets or sets the weight/importance of the pillar.
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Gets or sets the collection of Key Performance Areas (KPAs) within this pillar.
        /// </summary>
        public List<KPADto>? KPA { get; set; }
    }

    /// <summary>
    /// Represents a Key Performance Area (KPA) with its associated KPIs.
    /// </summary>
    public class KPADto
    {
        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the KPA.
        /// </summary>
        public string? UUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the contract pillar.
        /// </summary>
        public string? ContractPillarsUUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the contract pillar KPA.
        /// </summary>
        public string? ContractPillarKPAsUUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the status.
        /// </summary>
        public string? StatusUUID { get; set; }

        /// <summary>
        /// Gets or sets the status name.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the rating period.
        /// </summary>
        public string? RatingPeriodsUUID { get; set; }

        /// <summary>
        /// Gets or sets the rating period name.
        /// </summary>
        public string? RatingPeriods { get; set; }

        /// <summary>
        /// Gets or sets the display name for the rating period.
        /// </summary>
        public string? RatingPeriodsDisplay { get; set; }

        /// <summary>
        /// Gets or sets the rating period dates (e.g., "Q3").
        /// </summary>
        public string? RatingPeriodDates { get; set; }

        /// <summary>
        /// Gets or sets the name of the KPA.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the KPA.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the weight/importance of the KPA.
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the KPA is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        public string? Template { get; set; }

        /// <summary>
        /// Gets or sets the status score level (e.g., "Level 3").
        /// </summary>
        public string? StatusScore { get; set; }

        /// <summary>
        /// Gets or sets the color code for the status score.
        /// </summary>
        public string? StatusScoreColor { get; set; }

        /// <summary>
        /// Gets or sets the employee's score.
        /// </summary>
        public decimal? ScoreEmployee { get; set; }

        /// <summary>
        /// Gets or sets the manager's score.
        /// </summary>
        public decimal? ScoreManager { get; set; }

        /// <summary>
        /// Gets or sets the collection of KPIs belonging to the same user.
        /// </summary>
        public List<KPIDto>? KPI { get; set; }

        /// <summary>
        /// Gets or sets the collection of KPIs linked from other users.
        /// </summary>
        public List<KPILinkedDto>? KPILinked { get; set; }
    }

    /// <summary>
    /// Represents a Key Performance Indicator (KPI).
    /// </summary>
    public class KPIDto
    {
        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the KPI.
        /// </summary>
        public string? UUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the KPA.
        /// </summary>
        public string? KPAUUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the KPA-KPI link.
        /// </summary>
        public string? KPAKPIUUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the company.
        /// </summary>
        public string? CompanyUUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the user.
        /// </summary>
        public string? UsersUUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the rating period.
        /// </summary>
        public string? RatingPeriodsUUID { get; set; }

        /// <summary>
        /// Gets or sets the rating period name.
        /// </summary>
        public string? RatingPeriods { get; set; }

        /// <summary>
        /// Gets or sets the display name for the rating period.
        /// </summary>
        public string? RatingPeriodsDisplay { get; set; }

        /// <summary>
        /// Gets or sets the rating period dates (e.g., "Q3").
        /// </summary>
        public string? RatingPeriodDates { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the status.
        /// </summary>
        public string? StatusUUID { get; set; }

        /// <summary>
        /// Gets or sets the status name.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the tolerance set.
        /// </summary>
        public string? ToleranceSetsUUID { get; set; }

        /// <summary>
        /// Gets or sets the tolerance set name.
        /// </summary>
        public string? ToleranceSets { get; set; }

        /// <summary>
        /// Gets or sets the description of the tolerance set.
        /// </summary>
        public string? ToleranceSetsDescription { get; set; }

        /// <summary>
        /// Gets or sets the name of the KPI.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the KPI.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the start date of the KPI.
        /// </summary>
        public DateTime? DateStart { get; set; }

        /// <summary>
        /// Gets or sets the end date of the KPI.
        /// </summary>
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// Gets or sets the target value for the KPI.
        /// </summary>
        public decimal? Target { get; set; }


        /// <summary>
        /// Gets or sets the current progress value for the KPI.
        /// </summary>
        public decimal? currentResultProgress { get; set; }
        /// <summary>
        /// Gets or sets the weight/importance of the KPI.
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the KPI is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        public string? Template { get; set; }

        /// <summary>
        /// Gets or sets the status score level (e.g., "Level 3").
        /// </summary>
        public string? StatusScore { get; set; }

        /// <summary>
        /// Gets or sets the color code for the status score.
        /// </summary>
        public string? StatusScoreColor { get; set; }

        /// <summary>
        /// Gets or sets the employee's score.
        /// </summary>
        public decimal? ScoreEmployee { get; set; }

        /// <summary>
        /// Gets or sets the manager's score.
        /// </summary>
        public decimal? ScoreManager { get; set; }

        /// <summary>
        /// Progress graph entries (Result history)
        /// </summary>
        public List<ProgressGraphDto>? ProgressGraphData { get; set; }
    }
    public class ProgressGraphDto
    {
        public DateTime? DateAdded { get; set; }
        public decimal? Result { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Represents a linked Key Performance Indicator (KPI) from another user.
    /// </summary>
    public class KPILinkedDto
    {
        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the KPI.
        /// </summary>
        public string? UUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the company.
        /// </summary>
        public string? CompanyUUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the user.
        /// </summary>
        public string? UsersUUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the contract pillar KPA.
        /// </summary>
        public string? ContractPillarKPAsUUID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the status.
        /// </summary>
        public string? StatusUUID { get; set; }

        /// <summary>
        /// Gets or sets the status name.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the rating period.
        /// </summary>
        public string? RatingPeriodsUUID { get; set; }

        /// <summary>
        /// Gets or sets the rating period name.
        /// </summary>
        public string? RatingPeriods { get; set; }

        /// <summary>
        /// Gets or sets the display name for the rating period.
        /// </summary>
        public string? RatingPeriodsDisplay { get; set; }

        /// <summary>
        /// Gets or sets the rating period dates (e.g., "Q3").
        /// </summary>
        public string? RatingPeriodDates { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier (UUID) of the tolerance set.
        /// </summary>
        public string? ToleranceSetsUUID { get; set; }

        /// <summary>
        /// Gets or sets the tolerance set name.
        /// </summary>
        public string? ToleranceSets { get; set; }

        /// <summary>
        /// Gets or sets the description of the tolerance set.
        /// </summary>
        public string? ToleranceSetsDescription { get; set; }

        /// <summary>
        /// Gets or sets the name of the KPI.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the KPI.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the start date of the KPI.
        /// </summary>
        public DateTime? DateStart { get; set; }

        /// <summary>
        /// Gets or sets the end date of the KPI.
        /// </summary>
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// Gets or sets the target value for the KPI.
        /// </summary>
        public decimal? Target { get; set; }

        /// <summary>
        /// Gets or sets the weight/importance of the KPI.
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        public string? Template { get; set; }

        /// <summary>
        /// Gets or sets the status score level (e.g., "Level 3").
        /// </summary>
        public string? StatusScore { get; set; }

        /// <summary>
        /// Gets or sets the color code for the status score.
        /// </summary>
        public string? StatusScoreColor { get; set; }

        /// <summary>
        /// Gets or sets the employee's score.
        /// </summary>
        public decimal? ScoreEmployee { get; set; }

        /// <summary>
        /// Gets or sets the manager's score.
        /// </summary>
        public decimal? ScoreManager { get; set; }
    }
}
