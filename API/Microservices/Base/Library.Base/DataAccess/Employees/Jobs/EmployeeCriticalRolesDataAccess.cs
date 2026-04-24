using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.DataAccess.Employees.Jobs;

internal static class EmployeeCriticalRolesDataAccess
{
    public static async Task<IEnumerable<Models.Employees.EmployeeCriticalRolesModel>?> GetEmployeeCriticalRoles(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedIn) =>
        await sql.LoadDataAsync<Models.Employees.EmployeeCriticalRolesModel, dynamic>("[base].[spEmployeeCriticalRolesUUID]", new { companyUUID, usersUUIDLoggedIn });
}
