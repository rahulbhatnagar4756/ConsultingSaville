using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects.Groups
{
    public class JobGroupModel
    {
        public int? JobsId { get; set; }
        public string? JobsUUID { get; set; }
        public string? JobsName { get; set; }
        public int? JobRolesid { get; set; }
        public string? JobRolesUUID { get; set; }
        public string? JobRolesName { get; set; }
    }
}