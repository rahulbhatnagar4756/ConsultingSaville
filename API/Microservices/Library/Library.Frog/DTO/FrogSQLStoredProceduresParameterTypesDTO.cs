using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.DTO
{
    internal class FrogSQLStoredProceduresParameterTypesDTO
    {
        public string? FrogSQLStoredProceduresUUID { get; set; }
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDLoggedIn { get; set; }
        public string? FrogsUUID { get; set; }
    }
}
