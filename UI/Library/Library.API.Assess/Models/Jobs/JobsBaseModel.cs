using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Assess.Models.Jobs
{
    public class JobsBaseModel
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? JobType { get; set; }
        public int? isCritical { get; set; }
    }
}
