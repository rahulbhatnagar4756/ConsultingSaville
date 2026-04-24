using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.Tests;

public class TestAssignmentModel
{
    public string? ProjectUUID { get; set; }
    public string? CompanyUUID { get; set; }
    public string? AssessmentUsersUUID { get; set; }
    public string? TestsUUID { get; set; }
    public string? TestName { get; set; }
    public string? TestTypesUUID { get; set; }
    public string? IntegrationUUID { get; set; }
    public string? LanguageUUID { get; set; }
    public string? LanguageName { get; set; }
    public string? LanguageCode { get; set; }
}

public class TestAssignmentResultModel
{
    public List<TestAssignmentModel>? TestAssignments { get; set; }
    public ResultsModel Results { get; set; } = new ResultsModel();
}
