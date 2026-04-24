using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.Tolerances;

public class ToleranceSetsModel
{
    public string? UUID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public float MaxTotal { get; set; } = 100;
    public int OrderVal { get; set; } = 500;
    public string Ranges { get; set; }
}
