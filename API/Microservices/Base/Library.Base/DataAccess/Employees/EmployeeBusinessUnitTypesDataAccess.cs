using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Base.Models.Employees.BusinessStructure;

namespace Library.Base.DataAccess.Employees
{
    internal static class EmployeeBusinessUnitTypesDataAccess
    {
        public static async Task<IEnumerable<TypesModel>> GetEmployeeBusinessUnitTypes(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
            await sql.LoadDataAsync<TypesModel, dynamic>("[base].[spEmployeeBusinessUnitTypesUUID]", basic);

        /// <summary>
        /// gets the BusinessUnitTypes and the count of Business Units
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="basic"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<EmployeeBaseModel>> GetEmployeeBusinessUnitTypesByCompany(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
            await sql.LoadDataAsync<EmployeeBaseModel, dynamic>("[base].[spEmployeeBusinessUnitTypes]", basic);


        internal static async Task<IEnumerable<ResultsModel>> SaveEmployeeBusinessUnitTypes(ISqlDataAccess sql, EmployeeBusinessUnitTypesSaveModel save) =>
            await sql.LoadDataAsync<ResultsModel, dynamic>("[base].[spEmployeeBusinessUnitTypesSave]", save);
   
        internal static async Task<IEnumerable<ResultsModel>> DeleteEmployeeBusinessUnitTypes(ISqlDataAccess sql, DeleteModel delete) =>
            await sql.LoadDataAsync<ResultsModel, DeleteModel>("[base].[spEmployeeBusinessUnitTypesDelete]", delete);

    }
}
