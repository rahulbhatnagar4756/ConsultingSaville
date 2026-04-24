using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.EnterpriseStructures
{
    public class EnterpriseStructureTypesModel
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string Icons { get; set; } = "business";
        public string Color { get; set; } = "#362f21";

        // For UI compatibility with MultiListExtra
        public string? Icon { get; set; }
        public string IconColor { get; set; } = "#362f21";
        public int Count { get; set; } = 0; // For stats display
    }
}