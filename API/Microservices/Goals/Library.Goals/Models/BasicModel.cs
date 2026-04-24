using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models
{
    public class BasicModel : IBasicModel
    {
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDLoggedIn { get; set; }
    }
}
