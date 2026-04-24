using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects;

/// <summary>
/// Result model for candidate auto-login processing
/// </summary>
public class CandidateAutoLoginResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? CompanyUUID { get; set; }
    public string? ProjectsUUID { get; set; }
    public string? UsersUUID { get; set; }
    public string? Token { get; set; }
}
