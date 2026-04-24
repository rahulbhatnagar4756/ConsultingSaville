using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects.Groups
{
    public class AssessmentStatsGroupModel
    {
        public int Total { get; set; }
        public int NotStarted { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }
        
        public decimal CompletionPercentage => Total > 0 ? (decimal)Completed / Total * 100 : 0;
        public decimal InProgressPercentage => Total > 0 ? (decimal)InProgress / Total * 100 : 0;
        public decimal NotStartedPercentage => Total > 0 ? (decimal)NotStarted / Total * 100 : 0;
    }
}