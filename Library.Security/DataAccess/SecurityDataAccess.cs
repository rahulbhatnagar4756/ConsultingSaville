using Library.Database.DAL;
using Library.Security.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Security.DataAccess;

internal class SecurityDataAccess
{

    /// <summary>
    /// This method is used to login a user into the specified company
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="CompanyUUID"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<AuthResponseDTO?> LoginAsync(ISqlDataAccess sql,string CompanyUUID, string username, string password)
    {
        var data = await sql.LoadDataAsync<Security.DTO.AuthResponseDTO, dynamic>("[Securities].[spUsers_Login]", new { CompanyidUUID = CompanyUUID, Username = username, Password = password });
        if (data.Count() == 0) return new Security.DTO.AuthResponseDTO { IsAuthSuccessful = false, ErrorMessage = "Authentication failed" };
        return data.FirstOrDefault();
    }
}