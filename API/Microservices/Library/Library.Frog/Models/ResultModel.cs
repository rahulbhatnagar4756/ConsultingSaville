using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.Models
{
    public class ResultModel
    {
        public string? UUID { get; set; }
        public bool? isSuccess { get; set; }
        public string? Code { get; set; }
        public string? Message { get; set; }

    }
}
