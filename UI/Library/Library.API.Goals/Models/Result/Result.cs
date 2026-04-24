using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.Result
{
    public class ResultSaveModel
    {
        public string? Id { get; set; }                 // UUID (NULL = INSERT)
        public string KPI_UUID { get; set; }            // UUID
        public string RatingPeriod_UUID { get; set; }   // UUID
        public decimal ResultValue { get; set; }
        public bool isActive { get; set; }
    }
}
