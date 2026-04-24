using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.DTO
{
    internal class FrogSQLStoredProceduresDTO
    {
        public string? FrogSQLStoredProceduresUUID { get; set; }
        public string? StoredProcedure { get; set; }
        public int? FrogSQLStoredProceduresParameterTypesid { get; set; }
        public string? Parameters { get; set; }
        public string? DefaultValue { get; set; }
        public int? OrderVal { get; set; }

    }
}
