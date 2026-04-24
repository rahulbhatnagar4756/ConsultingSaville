using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.Contracts
{
    public class ContractsBasicModel
    {
        public string? ContractPeriods { get; set; }
        public string? ContractsUUID { get; set; } 
        public string? ContractPeriodsUUID { get; set; } 
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? Year { get; set; }
        public bool isDepartmentTemplateSync { get; set; }
        public bool isIndividualTemplateSync { get; set; }
        public bool isActive { get; set; } 

    }
}
