using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.PerformanceReview
{
    public class PerformanceReviewResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public byte[]? PdfData { get; set; }
        public string? FileName { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
