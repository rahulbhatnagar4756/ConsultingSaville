using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.Status
{
    public class StatusRangesModel
    {
        public string? UUID { get; set; }
        public string StatusUUID { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string Icons { get; set; } = "clock_loader_40";
        public string Color { get; set; } = "#362f21";
        public decimal? RangeStart { get; set; }
        public decimal? RangeEnd { get; set; } 
        public int isNegative { get; set; } = 0; 
    }

    public class StatusRangesSaveModel : IBasicModel
    {
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDLoggedIn { get; set; }
        public string? UUID { get; set; }
        public string StatusUUID { get; set; } = string.Empty; 
        public string Name { get; set; } = string.Empty;
        public string Icons { get; set; } = "clock_loader_40";
        public string Color { get; set; } = "#362f21";
        public decimal? RangeStart { get; set; } 
        public int isNegative { get; set; } = 0;
        public int OrderVal { get; set; } = 0;
    }
}
