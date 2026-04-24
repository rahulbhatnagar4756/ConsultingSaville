using Library.Base.DataAccess.Employees.Jobs;
using Library.Base.Mapper.BusinessStructure;
using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Base.Models.Employees.Jobs;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Services.Employees.Jobs;

public interface IEmployeeJobsCriticalRolesService
{
    Task<List<EmployeeJobsCriticalRoleBaseModel>?> GetEmployeeJobsCriticalRoles(EmployeeBasicGetModel basic);
    Task<ResultsModel?> SaveEmployeeJobsCriticalRoles(EmployeeBasicGetModel basic, EmployeeJobsCriticalRoleBaseModel criticalRole);
    Task<ResultsModel?> DeleteEmployeeJobsCriticalRoles(EmployeeBasicGetModel basic, string uuid);
}

public class EmployeeJobsCriticalRolesService : IEmployeeJobsCriticalRolesService
{
    private readonly ISqlDataAccess _sql;

    public EmployeeJobsCriticalRolesService(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public async Task<List<EmployeeJobsCriticalRoleBaseModel>?> GetEmployeeJobsCriticalRoles(EmployeeBasicGetModel basic)
    {
        var data = await EmployeeJobsCriticalRolesDataAccess.GetEmployeeJobsCriticalRolesBase(_sql, basic);
        if (data == null || !data.Any()) return null;
        return data.ToList();
    }

    public async Task<ResultsModel?> SaveEmployeeJobsCriticalRoles(EmployeeBasicGetModel basic, EmployeeJobsCriticalRoleBaseModel criticalRole)
    {
        if (criticalRole == null) return new ResultsModel { isValid = false, Message = "Invalid critical role data." };

        var saveModel = criticalRole.ToEmployeeJobsCriticalRoleSaveModel(basic);
        var result = await EmployeeJobsCriticalRolesDataAccess.SaveEmployeeJobsCriticalRole(_sql, saveModel);
        return result?.FirstOrDefault();
    }

    public async Task<ResultsModel?> DeleteEmployeeJobsCriticalRoles(EmployeeBasicGetModel basic, string uuid)
    {
        if (string.IsNullOrEmpty(uuid)) return new ResultsModel { isValid = false, Message = "Invalid critical role selected." };

        DeleteModel delete = new DeleteModel
        {
            CompanyUUID = basic.CompanyUUID,
            UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
            UUID = uuid
        };

        var result = await EmployeeJobsCriticalRolesDataAccess.DeleteEmployeeJobsCriticalRole(_sql, delete);
        return result?.FirstOrDefault();
    }
}
