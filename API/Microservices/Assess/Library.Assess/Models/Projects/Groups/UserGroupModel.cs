using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects.Groups
{
    public class UserGroupModel
    {
        public int? UsersidCreatedBy { get; set; }
        public string? UsersUUIDCreatedBy { get; set; }
        public string? UsersCreatedByFirstName { get; set; }
        public string? UsersCreatedByLastName { get; set; }
        public string? UsersCreatedByEmail { get; set; }
        public string? UsersCreatedByIDNumber { get; set; }
        public int? UsersidReleasedBy { get; set; }
        public bool? isSuperAdministrator { get; set; }
        
        public string? FullName => $"{UsersCreatedByFirstName} {UsersCreatedByLastName}".Trim();
    }
}