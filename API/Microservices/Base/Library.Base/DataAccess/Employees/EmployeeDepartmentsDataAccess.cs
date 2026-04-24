using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Base.Models.Employees.BusinessStructure;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.DataAccess.Employees
{
    internal static class EmployeeDepartmentsDataAccess
    {
        public static async Task<IEnumerable<TypesModel>> GetEmployeeDepartments(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
            await sql.LoadDataAsync<TypesModel, dynamic>("[base].[spEmployeeDepartmentsUUID]", basic);

        /// <summary>
        /// data returned is a list of employee departments by company 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="basic"></param>
        /// <returns></returns>
        internal static async Task<IEnumerable<EmployeeDepartmentBaseModel>> GetEmployeeDepartmentsBasic(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
            await sql.LoadDataAsync<EmployeeDepartmentBaseModel, dynamic>("[base].[spEmployeeDepartments_Basic]", basic);

        internal static async Task<IEnumerable<ResultsModel>> SaveEmployeeDepartment(ISqlDataAccess sql, EmployeeDepartmentsSaveModel department) =>
            await sql.LoadDataAsync<ResultsModel, dynamic>("[base].[spEmployeeDepartmentsSave]", department );

        internal static async Task<IEnumerable<ResultsModel>> DeleteEmployeeDepartment(ISqlDataAccess sql, DeleteModel delete) =>
            await sql.LoadDataAsync<ResultsModel, DeleteModel>("[base].[spEmployeeDepartmentsDelete]", delete);
    }
}
