using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects.Groups
{
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
}