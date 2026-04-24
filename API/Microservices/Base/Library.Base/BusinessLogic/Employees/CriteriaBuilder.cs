using Library.Base.DataAccess.Employees.Jobs;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.BusinessLogic.Employees
{
    internal class CriteriaBuilder
    {
        private Models.Employees.EmployeeSearchResultsModel _employeeSearchResults = new Models.Employees.EmployeeSearchResultsModel();
        private ISqlDataAccess _sql;

        public CriteriaBuilder(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public async Task<Models.Employees.EmployeeSearchResultsModel> GetCriteriaData(Models.Employees.EmployeeBasicGetModel Search)
        {
            //Department(search);
            //take all the private methods in the class add place them in a Task array[] so they all can be executed at the same time but wait till all have finished executing
            Task[] tasks = new Task[]
            {
                BusinessUnitTypes(Search),
                BusinessUnits(Search),
                Departments(Search),
                EmployeeJobs(Search),
                Disciplines(Search),
                CriticalRoles(Search),
                Levels(Search),
                Gender(Search),
                Ethnicity(Search),
                YearsOfExperience(Search),
                Age(Search)
            };

            await Task.WhenAll(tasks);

            // Code to get employees
            return _employeeSearchResults;
        }

        private async Task BusinessUnitTypes(Models.Employees.EmployeeBasicGetModel Search)
        {
            var data = await DataAccess.Employees.EmployeeBusinessUnitTypesDataAccess.GetEmployeeBusinessUnitTypes(_sql, Search);
            if (data == null || data.Count() == 0) return;
            _employeeSearchResults.BusinessUnitTypes = data.ToList();
        }

        private async Task BusinessUnits(Models.Employees.EmployeeBasicGetModel Search)
        {
            var data = await DataAccess.Employees.EmployeeBusinessUnitsDataAccess.GetEmployeeBusinessUnits(_sql, Search);
            if (data == null || data.Count() == 0) return;
            _employeeSearchResults.BusinessUnits = data.ToList();

        }

        private async Task Departments(Models.Employees.EmployeeBasicGetModel Search)
        {
            var data = await DataAccess.Employees.EmployeeDepartmentsDataAccess.GetEmployeeDepartments(_sql, Search);
            if (data == null || data.Count() == 0) return;
            _employeeSearchResults.Departments = data.ToList();
        }

        private async Task EmployeeJobs(Models.Employees.EmployeeBasicGetModel Search)
        {
            var data = await EmployeeJobsDataAccess.GetEmployeeJobs(_sql, Search);
            if (data == null || data.Count() == 0) return;
            _employeeSearchResults.Position = data.ToList();

        }

        private async Task Disciplines(Models.Employees.EmployeeBasicGetModel Search)
        {
            var data = await EmployeeJobDisciplinesDataAccess.GetEmployeeJobDisciplines(_sql, Search);
            if (data == null || data.Count() == 0) return;
            _employeeSearchResults.Disciplines = data.ToList();

        }

        private async Task CriticalRoles(Models.Employees.EmployeeBasicGetModel Search)
        {
            var data = await EmployeeJobsCriticalRolesDataAccess.GetEmployeeJobsCriticalRoles(_sql, Search);
            if (data == null || data.Count() == 0) return;
            _employeeSearchResults.CriticalRoles = data.ToList();

        }

        private async Task Levels(Models.Employees.EmployeeBasicGetModel Search)
        {
            var data = await EmployeeLevelsDataAccess.GetEmployeeLevels(_sql, Search);
            if (data == null || data.Count() == 0) return;
            _employeeSearchResults.Level = data.ToList();
        }

        private async Task Gender(Models.Employees.EmployeeBasicGetModel Search)
        {
            if (Search == null) return;
            var data = await DataAccess.Users.UsersDataAccess.GetGender (_sql, Search.CompanyUUID, Search.UsersUUIDLoggedIn);
            if (data == null || data.Count() == 0) return;
            _employeeSearchResults.Gender = data.ToList();

        }

        private async Task Ethnicity(Models.Employees.EmployeeBasicGetModel Search)
        {
            var data = await DataAccess.Users.UsersDataAccess.GetEthnicity(_sql, Search.CompanyUUID, Search.UsersUUIDLoggedIn);
            if (data == null || data.Count() == 0) return;
            _employeeSearchResults.Ethnicity = data.ToList();

        }

        private async Task YearsOfExperience(Models.Employees.EmployeeBasicGetModel Search)
        {
            var data = await DataAccess.Employees.EmployeeYearsOfExperienceDataAccess.GetEmployeeYearsOfExperience(_sql, Search);
            if (data == null || data.Count() == 0) return;
            _employeeSearchResults.YearsOfExperience = data.ToList();

        }
        
        private async Task Age(Models.Employees.EmployeeBasicGetModel Search)
        {
            var Data = await EmployeeJobsDataAccess.GetEmployeeAges(_sql, Search);
            if (Data == null || Data.Count() == 0) return;
            _employeeSearchResults.Age = Data.ToList();
        }



    }
}
