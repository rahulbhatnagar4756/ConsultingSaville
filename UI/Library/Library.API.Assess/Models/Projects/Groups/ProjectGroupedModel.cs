using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Assess.Models.Projects.Groups
{
    public class ProjectGroupedModel
    {
        public ProjectGroupModel? Project { get; set; }
        public UserGroupModel? User { get; set; }
        public AccessGroupModel? Access { get; set; }
        public StatusGroupModel? Status { get; set; }
        public AssessmentStatsGroupModel? AssessmentStats { get; set; }
        public JobGroupModel? Job { get; set; }
        public ProjectTypeGroupModel? ProjectType { get; set; }
        public LanguageGroupModel? Language { get; set; }
    }

    public class ProjectGroupModel
    {
        public int Id { get; set; }
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateReleased { get; set; }
        public DateTime? DateClosing { get; set; }
        public TimeSpan? TimeAllowedStart { get; set; }
        public TimeSpan? TimeAllowedEnd { get; set; }
        public bool? isPublicLink { get; set; }
        public bool? isAutoCommCandidateLink { get; set; }
        public string? Company { get; set; }
        public string? CompanyUUID { get; set; }
    }

    public class UserGroupModel
    {
        public int? UsersidCreatedBy { get; set; }
        public string? UsersUUIDCreatedBy { get; set; }
        public string? UsersCreatedByFirstName { get; set; }
        public string? UsersCreatedByLastName { get; set; }
        public string? UsersCreatedByEmail { get; set; }
        public string? UsersCreatedByIDNumber { get; set; }
        public int? UsersidReleasedBy { get; set; }
        public bool? isSuperAdministrator { get; set; }
        
        public string? FullName => $"{UsersCreatedByFirstName} {UsersCreatedByLastName}".Trim();
    }

    public class AccessGroupModel
    {
        public bool? isAccessRead { get; set; }
        public bool? isAccessEdit { get; set; }
        public bool? isAccessDelete { get; set; }
        public bool? isSuperAdministrator { get; set; }
        
        public bool HasAnyAccess => isAccessRead == true || isAccessEdit == true || isAccessDelete == true || isSuperAdministrator == true;
    }

    public class StatusGroupModel
    {
        public bool? isWaiting { get; set; }
        public bool? isReleased { get; set; }
        public bool? isCompleted { get; set; }
        public bool? isArchive { get; set; }
        public bool? isDeleted { get; set; }
        
        public string CurrentStatus
        {
            get
            {
                if (isDeleted == true) return "Deleted";
                if (isArchive == true) return "Archived";
                if (isCompleted == true) return "Completed";
                if (isReleased == true) return "Released";
                if (isWaiting == true) return "Waiting";
                return "Unknown";
            }
        }
    }

    public class AssessmentStatsGroupModel
    {
        public int Total { get; set; }
        public int NotStarted { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }
        
        public decimal CompletionPercentage => Total > 0 ? (decimal)Completed / Total * 100 : 0;
        public decimal InProgressPercentage => Total > 0 ? (decimal)InProgress / Total * 100 : 0;
        public decimal NotStartedPercentage => Total > 0 ? (decimal)NotStarted / Total * 100 : 0;
    }

    public class JobGroupModel
    {
        public int? JobsId { get; set; }
        public string? JobsUUID { get; set; }
        public string? JobsName { get; set; }
        public int? JobRolesid { get; set; }
        public string? JobRolesUUID { get; set; }
        public string? JobRolesName { get; set; }
    }

    public class ProjectTypeGroupModel
    {
        public int? ProjectTypesid { get; set; }
        public string? ProjectTypesUUID { get; set; }
        public string? ProjectTypesName { get; set; }
    }

    public class LanguageGroupModel
    {
        public int? Languageid { get; set; }
        public string? LanguageUUID { get; set; }
        public string? LanguageName { get; set; }
        public string? LanguageCode { get; set; }
    }
}