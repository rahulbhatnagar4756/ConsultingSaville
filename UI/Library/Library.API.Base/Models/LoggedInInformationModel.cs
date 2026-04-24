using Library.API.Base.Models;
using Library.API.Base.Models.Users;
using System;
using System.Reflection;

namespace OneCoreAssessUI.Services;

public class LoggedInInformationModel
{
    public CompanyBasicModel? Company { get; set; } 
    public UserBasicModel? User { get; set; }
   
    public bool isGhost { get; set; } = false;
    public UserBasicModel? UserGhost { get; set; }

}
