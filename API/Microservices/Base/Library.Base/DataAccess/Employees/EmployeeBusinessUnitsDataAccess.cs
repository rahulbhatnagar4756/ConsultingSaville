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
    internal static class EmployeeBusinessUnitsDataAccess
    {

        public static async Task<IEnumerable<TypesModel>> GetEmployeeBusinessUnits(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
            await sql.LoadDataAsync<TypesModel, dynamic>("[base].[spEmployeeBusinessUnitsUUID]", basic);


        internal static async Task<IEnumerable<EmployeeBaseModel>> GetEmployeeBusinessUnitsByCompany(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
            await sql.LoadDataAsync<EmployeeBaseModel, dynamic>("[base].[spEmployeeBusinessUnits]", basic);

        internal static async Task<IEnumerable<ResultsModel>> SaveEmployeeBusinessUnit(ISqlDataAccess sql, EmployeeBusinessUnitsSaveModel businessUnit) =>
            await sql.LoadDataAsync<ResultsModel, dynamic>("[base].[spEmployeeBusinessUnitSave]", businessUnit);

        internal static async Task<IEnumerable<ResultsModel>> DeleteEmployeeBusinessUnit(ISqlDataAccess sql, DeleteModel delete) =>
            await sql.LoadDataAsync<ResultsModel, DeleteModel>("[base].[spEmployeeBusinessUnitDelete]", delete);



    }
}
