using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Projects;

public class CandidateRegistrationModel
{
    public string? CompanyUUID { get; set; }
    public string? ProjectsUUID { get; set; }
    public string? IDNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string? Gender { get; set; }
    public string? Ethnicity { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool isSuccess { get; set; }
    public string? Message { get; set; }
}


