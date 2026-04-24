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
    internal static class EmployeeYearsOfExperienceDataAccess
    {
        public static async Task<IEnumerable<TypesModel>> GetEmployeeYearsOfExperience(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
            await sql.LoadDataAsync<TypesModel, dynamic>("[base].[spEmployeeYearsOfExperienceUUID]", basic);
    }
}
