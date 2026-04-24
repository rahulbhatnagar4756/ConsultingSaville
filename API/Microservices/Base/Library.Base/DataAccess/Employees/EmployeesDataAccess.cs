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
    internal static class EmployeesDataAccess
    {

        public static async Task<IEnumerable<EmployeeModel>> EmployeeFilterByJson(ISqlDataAccess sql, string CompanyUUID, string UsersUUIDLoggedIn, string Json) =>
            await sql.LoadDataAsync<EmployeeModel, dynamic>("[base].[spEmployees_Filter_Json]", new { CompanyUUID, UsersUUIDLoggedIn, Json });


        public static async Task<EmployeeBasicInformationModel?> EmployeeInformationByUsersUUID(ISqlDataAccess sql, string CompanyUUID, string UsersUUIDLoggedIn, string usersUUID)
        {
         var Data =  await sql.LoadDataAsync<EmployeeBasicInformationModel, dynamic>("[base].[spEmployees_Basic_Information]", new { CompanyUUID, UsersUUIDLoggedIn, usersUUID });
            if (Data == null) return null;
            return Data.FirstOrDefault();
        }

        internal static async Task<ResultsModel?> SaveEmployeeBasicInformation(ISqlDataAccess sql, EmployeeBasicInformationModel employeeInformation)=>
            await sql.LoadFirstDataAsync<Models.ResultsModel, dynamic>("[base].[spEmployees_Basic_Information_Save]", employeeInformation);

    }
}
