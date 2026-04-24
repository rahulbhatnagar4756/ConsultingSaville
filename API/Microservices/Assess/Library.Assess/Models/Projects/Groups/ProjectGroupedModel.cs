using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects.Groups
{
    /// <summary>
    /// Container model that groups all project-related data into logical sections
    /// </summary>
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
}
