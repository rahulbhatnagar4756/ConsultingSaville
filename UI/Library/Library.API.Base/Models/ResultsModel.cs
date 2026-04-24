using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models
{
    public class ResultsModel
    {
        public string? UUID { get; set; }
        public bool? isValid { get; set; } = false;
        public string? Message { get; set; }
    }
}
