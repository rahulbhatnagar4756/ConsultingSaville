using Library.Base.Mapper.BusinessStructure;
using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Base.Models.Employees.BusinessStructure;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Services.Employees
{
    public interface IEmployeeDepartmentsService
    {
        Task<List<EmployeeDepartmentBaseModel>?> GetEmployeeDepartments(EmployeeBasicGetModel basic);
        Task<ResultsModel?> SaveEmployeeDepartments(EmployeeBasicGetModel basic, EmployeeDepartmentBaseModel department);
        Task<ResultsModel?> DeleteEmployeeDepartments(EmployeeBasicGetModel basic, string uuid);
    }

    public class EmployeeDepartmentsService : IEmployeeDepartmentsService
    {
        private readonly ISqlDataAccess _sql;

        public EmployeeDepartmentsService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public async Task<List<EmployeeDepartmentBaseModel>?> GetEmployeeDepartments(Models.Employees.EmployeeBasicGetModel basic)
        {
            var Data = await DataAccess.Employees.EmployeeDepartmentsDataAccess.GetEmployeeDepartmentsBasic(_sql, basic);
            if (Data == null || Data.Count() == 0) return default;
            return Data.ToList();
        }

        public async Task<ResultsModel?> SaveEmployeeDepartments(Models.Employees.EmployeeBasicGetModel basic, EmployeeDepartmentBaseModel department)
        {
            if (department == null) return new ResultsModel { isValid = false, Message = "Invalid department data." };
            var DSave = department.ToEmployeeDepartmentsSaveModel(basic);
            var result = await DataAccess.Employees.EmployeeDepartmentsDataAccess.SaveEmployeeDepartment(_sql, DSave);
            return result?.FirstOrDefault()??default;
        }

        public async Task<ResultsModel?> DeleteEmployeeDepartments(Models.Employees.EmployeeBasicGetModel basic, string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return new ResultsModel { isValid = false, Message = "Invalid department selected." };
            DeleteModel delete = new DeleteModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            var result = await DataAccess.Employees.EmployeeDepartmentsDataAccess.DeleteEmployeeDepartment(_sql, delete);
            return result.FirstOrDefault();
        }
    }
}
