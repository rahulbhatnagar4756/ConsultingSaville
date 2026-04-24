using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.Status
{
    public class StatusModel
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int OrderVal { get; set; } = 0;
    }
}
