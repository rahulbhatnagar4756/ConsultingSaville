using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects
{
    public class ProjectSaveModel : BasicGetModel
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? JobsUUID { get; set; }
        public string? ProjectTypesUUID { get; set; }
        public string? JobRolesUUID { get; set; }
        public string? LanguageUUID { get; set; }
        public DateTime? DateClosing { get; set; }
        public TimeSpan? TimeAllowedStart { get; set; }
        public TimeSpan? TimeAllowedEnd { get; set; }
        public bool? isPublicLink { get; set; }
        public bool? isAutoCommCandidateLink { get; set; } 
        public bool? isReleased { get; set; }
    }
}
