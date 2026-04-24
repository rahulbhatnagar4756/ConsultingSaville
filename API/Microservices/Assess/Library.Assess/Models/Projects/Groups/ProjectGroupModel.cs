using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects.Groups
{
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
}