using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.Contracts;

public class ContractsCloneModel
{
    public string? ContractsUUID { get; set; }
    public string? TemplatesUUID { get; set; }
    public List<string>? UsersUUIDs { get; set; }
}
