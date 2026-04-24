using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library.Goals.Models.Templates;

public class TemplateBasicModel
{
    public string? TemplatesUUID { get; set; }
    public string? CompanyUUID { get; set; }
    public string? UsersUUIDLoggedIn { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
}
