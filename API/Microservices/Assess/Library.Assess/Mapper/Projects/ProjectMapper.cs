using Library.Assess.Models;
using Library.Assess.Models.Projects;
using Library.Assess.Models.Projects.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Mapper.Projects
{
    public static class ProjectMapper
    {
        /// <summary>
        /// Maps ProjectModel to grouped models for easier data consumption
        /// </summary>
        /// <param name="project">The main project model</param>
        /// <returns>ProjectGroupedModel containing all grouped data</returns>
        public static ProjectGroupedModel ToGroupedModel(this ProjectModel project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));

            return new ProjectGroupedModel
            {
                Project = project.ToProjectGroup(),
                User = project.ToUserGroup(),
                Access = project.ToAccessGroup(),   
                Status = project.ToStatusGroup(),
                AssessmentStats = project.ToAssessmentStatsGroup(),
                Job = project.ToJobGroup(),
                ProjectType = project.ToProjectTypeGroup(),
                Language = project.ToLanguageGroup()
            };
        }

        /// <summary>
        /// Maps a collection of ProjectModels to grouped models
        /// </summary>
        /// <param name="projects">Collection of project models</param>
        /// <returns>Collection of grouped project models</returns>
        public static IEnumerable<ProjectGroupedModel> ToGroupedModels(this IEnumerable<ProjectModel> projects)
        {
            return projects?.Select(p => p.ToGroupedModel()) ?? Enumerable.Empty<ProjectGroupedModel>();
        }

        /// <summary>
        /// Extracts project-specific information
        /// </summary>
        public static ProjectGroupModel ToProjectGroup(this ProjectModel project)
        {
            return new ProjectGroupModel
            {
                Id = project.Id,
                UUID = project.UUID,
                Name = project.Name,
                Description = project.Description,
                DateCreated = project.DateCreated,
                DateReleased = project.DateReleased,
                DateClosing = project.DateClosing,
                TimeAllowedStart = project.TimeAllowedStart,
                TimeAllowedEnd = project.TimeAllowedEnd,
                isPublicLink = project.isPublicLink,
                isAutoCommCandidateLink = project.isAutoCommCandidateLink,
                Company = project.Company,
                CompanyUUID = project.CompanyUUID
            };
        }

        /// <summary>
        /// Extracts user-related information
        /// </summary>
        public static UserGroupModel ToUserGroup(this ProjectModel project)
        {
            return new UserGroupModel
            {
                UsersidCreatedBy = project.UsersidCreatedBy,
                UsersUUIDCreatedBy = project.UsersUUIDCreatedBy,
                UsersCreatedByFirstName = project.UsersCreatedByFirstName,
                UsersCreatedByLastName = project.UsersCreatedByLastName,
                UsersCreatedByEmail = project.UsersCreatedByEmail,
                UsersCreatedByIDNumber = project.UsersCreatedByIDNumber,
                UsersidReleasedBy = project.UsersidReleasedBy,
                isSuperAdministrator = project.isSuperAdministrator
            };
        }

        /// <summary>
        /// Extracts access control information
        /// </summary>
        public static AccessGroupModel ToAccessGroup(this ProjectModel project)
        {
            return new AccessGroupModel
            {
                isAccessRead = project.isAccessRead,
                isAccessEdit = project.isAccessEdit,
                isAccessDelete = project.isAccessDelete,
                isSuperAdministrator = project.isSuperAdministrator
            };
        }

        /// <summary>
        /// Extracts status information
        /// </summary>
        public static StatusGroupModel ToStatusGroup(this ProjectModel project)
        {
            return new StatusGroupModel
            {
                isWaiting = project.isWaiting,
                isReleased = project.isReleased,
                isCompleted = project.isCompleted,
                isArchive = project.isArchive,
                isDeleted = project.isDeleted
            };
        }

        /// <summary>
        /// Extracts assessment statistics
        /// </summary>
        public static AssessmentStatsGroupModel ToAssessmentStatsGroup(this ProjectModel project)
        {
            return new AssessmentStatsGroupModel
            {
                Total = project.Total,
                NotStarted = project.NotStarted,
                InProgress = project.InProgress,
                Completed = project.Completed
            };
        }

        /// <summary>
        /// Extracts job-related information
        /// </summary>
        public static JobGroupModel ToJobGroup(this ProjectModel project)
        {
            return new JobGroupModel
            {
                JobsId = project.JobsId,
                JobsUUID = project.JobsUUID,
                JobsName = project.JobsName,
                JobRolesid = project.JobRolesid,
                JobRolesUUID = project.JobRolesUUID,
                JobRolesName = project.JobRolesName
            };
        }

        /// <summary>
        /// Extracts project type information
        /// </summary>
        public static ProjectTypeGroupModel ToProjectTypeGroup(this ProjectModel project)
        {
            return new ProjectTypeGroupModel
            {
                ProjectTypesid = project.ProjectTypesid,
                ProjectTypesUUID = project.ProjectTypesUUID,
                ProjectTypesName = project.ProjectTypesName
            };
        }

        /// <summary>
        /// Extracts language information
        /// </summary>
        public static LanguageGroupModel ToLanguageGroup(this ProjectModel project)
        {
            return new LanguageGroupModel
            {
                Languageid = project.Languageid,
                LanguageUUID = project.LanguageUUID,
                LanguageName = project.LanguageName,
                LanguageCode = project.LanguageCode
            };
        }

        /// <summary>
        /// Maps from save model to basic get model for service layer operations
        /// </summary>
        public static BasicGetModel ToBasicGetModel(this ProjectSaveModel saveModel)
        {
            return new BasicGetModel
            {
                CompanyUUID = saveModel.CompanyUUID,
                UsersUUIDLoggedIn = saveModel.UsersUUIDLoggedIn
            };
        }
    }
}