using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models.Users
{
    public class UserDataBasicModel
    {
        public string? EmployeeNumber { get; set; }
        public DateTime? DateJoined{ get; set; }
        public string? WorkPhone { get; set; }
    }
}
