using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects
{
    public class ProjectSearchModel : BasicGetModel
    {
        public string? Search { get; set; }
        public bool isWaiting { get; set; } = true;
        public bool isReleased { get; set; } = true;
        public bool isArchive { get; set; } = false;
        public bool isCompleted { get; set; } = false;
        public bool isDeleted { get; set; } = false;
        public int? ReturnNo { get; set; }
    }
}
