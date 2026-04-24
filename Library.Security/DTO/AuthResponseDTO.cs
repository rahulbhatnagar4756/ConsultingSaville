using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Security.DTO
{
    internal class AuthResponseDTO
    {
        public bool IsAuthSuccessful { get; set; }
        public string? CompanyUUID { get; set; }
        public string? UsersUUID { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
