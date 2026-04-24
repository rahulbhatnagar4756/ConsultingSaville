using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects.Groups
{
    public class AccessGroupModel
    {
        public bool? isAccessRead { get; set; }
        public bool? isAccessEdit { get; set; }
        public bool? isAccessDelete { get; set; }
        public bool? isSuperAdministrator { get; set; }
        
        public bool HasAnyAccess => isAccessRead == true || isAccessEdit == true || isAccessDelete == true || isSuperAdministrator == true;
    }
}