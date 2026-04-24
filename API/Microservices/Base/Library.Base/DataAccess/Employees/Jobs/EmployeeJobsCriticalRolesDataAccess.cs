using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Base.Models.Employees.Jobs;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.DataAccess.Employees.Jobs
{
    internal static class EmployeeJobsCriticalRolesDataAccess
    {
        public static async Task<IEnumerable<TypesModel>> GetEmployeeJobsCriticalRoles(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
            await sql.LoadDataAsync<TypesModel, dynamic>("[base].[spEmployeeJobsCriticalRolesUUID]", basic);

        public static async Task<IEnumerable<EmployeeJobsCriticalRoleBaseModel>> GetEmployeeJobsCriticalRolesBase(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
           await sql.LoadDataAsync<EmployeeJobsCriticalRoleBaseModel, dynamic>("[base].[spEmployeeJobsCriticalRolesBase]", basic);

        public static async Task<IEnumerable<ResultsModel>> SaveEmployeeJobsCriticalRole(ISqlDataAccess sql, EmployeeJobsCriticalRoleSaveModel model) =>
            await sql.LoadDataAsync<ResultsModel, EmployeeJobsCriticalRoleSaveModel>("[base].[spEmployeeJobsCriticalRolesSave]", model);

        public static async Task<IEnumerable<ResultsModel>> DeleteEmployeeJobsCriticalRole(ISqlDataAccess sql, DeleteModel model) =>
            await sql.LoadDataAsync<ResultsModel, DeleteModel>("[base].[spEmployeeJobsCriticalRolesDelete]", model);
    }
}
