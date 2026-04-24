using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.Tolerances
{
    public class ToleranceRangesModel
    {
        public string? UUID { get; set; }
        public string? ToleranceSetsUUID { get; set; }
        public decimal? RangeStart { get; set; }
        public decimal? RangeEnd { get; set; }
        public decimal Score { get; set; } = 0;
    }
}
