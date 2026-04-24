using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Base.Models.Employees.Jobs;
using Library.Database.DAL;
using Library.Database.Services;

namespace Library.Base.DataAccess.Employees.Jobs
{
    internal static class EmployeeJobDisciplinesDataAccess
    {

        public static async Task<IEnumerable<TypesModel>> GetEmployeeJobDisciplines(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
            await sql.LoadDataAsync<TypesModel, dynamic>("[base].[spEmployeeJobDisciplinesUUID]", basic);


        public static async Task<IEnumerable<EmployeeJobDisciplineBaseModel>> GetEmployeeJobDisciplinesBase(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
            await sql.LoadDataAsync<EmployeeJobDisciplineBaseModel, dynamic>("[base].[spEmployeeJobDisciplines]", basic);

        public static async Task<IEnumerable<ResultsModel>> SaveEmployeeJobDiscipline(ISqlDataAccess sql, EmployeeJobDisciplineSaveModel model) =>
            await sql.LoadDataAsync<ResultsModel, EmployeeJobDisciplineSaveModel>("[base].[spEmployeeJobDisciplinesSave]", model);

        public static async Task<IEnumerable<ResultsModel>> DeleteEmployeeJobDiscipline(ISqlDataAccess sql, DeleteModel model) =>
            await sql.LoadDataAsync<ResultsModel, DeleteModel>("[base].[spEmployeeJobDisciplinesDelete]", model);

    }
}
