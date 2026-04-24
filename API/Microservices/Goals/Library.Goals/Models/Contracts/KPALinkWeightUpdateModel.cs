using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.Contracts;

public class KPALinkWeightUpdateModel
{
    public string UUID { get; set; } = string.Empty;
    public decimal Weight { get; set; }
}

 