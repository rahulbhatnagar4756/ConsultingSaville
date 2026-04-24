using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models.Users
{
    public class UserSearchNameModel
    {
        public string Search { get; set; }
        public bool IsIncludeTeam { get; set; }
    }
}
