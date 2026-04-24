using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models
{
    public class ResultsModel
    {
        public string? UUID { get; set; }
        public bool isValid { get; set; }
        public string? Message { get; set; }
    }
}
