using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.EnterpriseStructures
{
    public class EnterpriseStructureTypesModel
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string Icons { get; set; } = "business";
        public string Color { get; set; } = "#362f21";
    }

    public class EnterpriseStructureTypesSaveModel : EnterpriseStructureTypesModel, IBasicModel
    {
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDLoggedIn { get; set; }
    }
}