using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models
{
    public class UserTemplatesModel
    {
        public string? TemplatesUUID { get; set; }
        public List<string>? UsersUUID { get; set; }
        public string? ContractPeriodsUUID { get; set; }
    }
}
