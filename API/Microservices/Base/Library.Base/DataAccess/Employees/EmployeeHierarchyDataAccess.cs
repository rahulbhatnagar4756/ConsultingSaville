using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.DataAccess.Employees
{
    internal static class EmployeeHierarchyDataAccess
    {
        public static async Task<IEnumerable<EmployeeHierarchyModel>?> EmployeeHierarchyByUsersUUID(ISqlDataAccess sql, string CompanyUUID, string UsersUUIDLoggedIn, string UsersUUID, bool isHistory) =>
            await sql.LoadDataAsync<EmployeeHierarchyModel, dynamic>("[base].[spEmployeeHierarchy_By_UsersUUID]", new { CompanyUUID, UsersUUIDLoggedIn, UsersUUID, isHistory });

        internal static async Task<ResultsModel> SaveEmployeeHierarchy(ISqlDataAccess sql, EmployeeHierarchyBasicModel employeeHierarchy) =>
            await sql.LoadFirstDataAsync<ResultsModel, dynamic>("[base].[spEmployeeHierarchy_Save]", employeeHierarchy);
                
        internal static async Task<ResultsModel> DeleteEmployeeHierarchy(ISqlDataAccess sql, string CompanyUUID, string UsersUUIDLoggedIn, string EmployeeHierarchyUUID) =>
            await sql.LoadFirstDataAsync<ResultsModel, dynamic>("[base].[spEmployeeHierarchy_Delete]", new { CompanyUUID, UsersUUIDLoggedIn, EmployeeHierarchyUUID });
    }
}
