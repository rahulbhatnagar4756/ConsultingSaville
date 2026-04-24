using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Models.Users;

public class UserBasicInformationModel
{
    public string? UUID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? IDNumber { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string? Gender { get; set; }
    public string? Ethnicity { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? UsersImageURL { get; set; }
    public byte[]? UsersImageBytes { get; set; }
}
