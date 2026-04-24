using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.Pillars
{
    public class PillarsModel
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Icons { get; set; } = "account_balance";
        public string IconColor { get; set; } = "#362f21";
        public int OrderVal { get; set; } = 0;
    }
}