using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.EmployeeResult
{
    public class ResultDto
    {
        public string Id { get; set; }                  // UUID
        public string KPI_UUID { get; set; }            // UUID
        public string RatingPeriod_UUID { get; set; }   // UUID
        public decimal Result { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateAdded { get; set; }
    }

    public class SaveResultRequest
    {
        public int Id { get; set; }                 // UUID (NULL = INSERT)
        public string KPI_UUID { get; set; }            // UUID
        public string RatingPeriod_UUID { get; set; }   // UUID
        public decimal ResultValue { get; set; }
        public bool isActive { get; set; }
    }

    public class SaveResultResponse
    {
        public int? Id { get; set; }
        public bool isValid { get; set; }
        public string? Message { get; set; }
    }
}
