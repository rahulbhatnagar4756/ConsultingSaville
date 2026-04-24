using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects
{
    public class ProjectStatusUpdateModel : BasicGetModel
    {
        public string? UUID { get; set; }
        public bool? isWaiting { get; set; }
        public bool? isReleased { get; set; }
        public bool? isCompleted { get; set; }
        public bool? isArchive { get; set; }
    }
}