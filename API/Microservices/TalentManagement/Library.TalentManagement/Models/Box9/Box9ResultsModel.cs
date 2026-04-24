using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TalentManagement.Models.Box9;

public class Box9ResultsModel
{

    public int? Box9ResultsId { get; set; }
    public int? CompanyId { get; set; }
    public string? CompanyUUID { get; set; }
    public int? UsersIdLoggedIn { get; set; }
    public int? UsersId { get; set; }
    public string? UsersUUID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? IDNumber { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? Email { get; set; }
    public int? JobsId { get; set; }
    public int? JobMatch { get; set; }
    public string? JobMatchCode { get; set; }
    public int? CPP { get; set; }
    public string? CPPCode { get; set; }
    public decimal? FinalGoalScore { get; set; }
    public DateTime? DateCreated { get; set; }
    public int? Year { get; set; }
    public int? Box9Id { get; set; }
    public string? Box9Code { get; set; }
    public int? OrderVal { get; set; }
    public string? Information { get; set; }
    public string? InformationHTML { get; set; }
    public bool? isDeleted { get; set; }


}
