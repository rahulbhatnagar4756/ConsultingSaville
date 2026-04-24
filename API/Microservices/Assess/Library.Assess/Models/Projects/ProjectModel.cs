using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects
{
    public class ProjectModel
    {
        // Primary Project Information
        public int Id { get; set; }
        public string? UUID { get; set; }
        public int? Iconsid { get; set; }
        public int? Companyid { get; set; }
        public string? CompanyUUID { get; set; }
        public string? Company { get; set; }
        public int? Languageid { get; set; }
        public string? LanguageUUID { get; set; }
        public string? LanguageName { get; set; }
        public string? LanguageCode { get; set; }
        
        // Created By User Information
        public int? UsersidCreatedBy { get; set; }
        public string? UsersUUIDCreatedBy { get; set; }
        public string? UsersCreatedByFirstName { get; set; }
        public string? UsersCreatedByLastName { get; set; }
        public string? UsersCreatedByEmail { get; set; }
        public string? UsersCreatedByIDNumber { get; set; }
        public int? UsersidReleasedBy { get; set; }
        
        // Job Information
        public int? JobsId { get; set; }
        public string? JobsUUID { get; set; }
        public string? JobsName { get; set; }
        
        // Project Type Information
        public int? ProjectTypesid { get; set; }
        public string? ProjectTypesUUID { get; set; }
        public string? ProjectTypesName { get; set; }
        
        // Job Role Information
        public int? JobRolesid { get; set; }
        public string? JobRolesUUID { get; set; }
        public string? JobRolesName { get; set; }
        
        // Project Details
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateReleased { get; set; }
        public DateTime? DateClosing { get; set; }
        public TimeSpan? TimeAllowedStart { get; set; }
        public TimeSpan? TimeAllowedEnd { get; set; }
        
        // Settings
        public bool? isPublicLink { get; set; }
        public bool? isAutoCommCandidateLink { get; set; } 
        
        // Status Flags
        public bool? isWaiting { get; set; }
        public bool? isReleased { get; set; }
        public bool? isCompleted { get; set; }
        public bool? isArchive { get; set; }
        public bool? isDeleted { get; set; }
        
        // Access Control
        public bool? isSuperAdministrator { get; set; }
        public bool? isAccessRead { get; set; }
        public bool? isAccessEdit { get; set; }
        public bool? isAccessDelete { get; set; }
        
        // Assessment Statistics
        public int Total { get; set; }
        public int NotStarted { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }
    }
}
