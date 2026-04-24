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

namespace Library.Base.Services.Employees;

public interface IEmployeeBusinessUnitTypesService
{
    Task<ResultsModel?> DeleteEmployeeBusinessUnitTypes(EmployeeBasicGetModel basic, string uuid);
    Task<List<EmployeeBaseModel>?> GetEmployeeBusinessUnitTypes(EmployeeBasicGetModel basic);
    Task<ResultsModel?> SaveEmployeeBusinessUnitTypes(EmployeeBasicGetModel basic, EmployeeBaseModel businessUnit);
}

public class EmployeeBusinessUnitTypesService : IEmployeeBusinessUnitTypesService
{
    private readonly ISqlDataAccess _sql;

    public EmployeeBusinessUnitTypesService(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public async Task<List<Models.Employees.BusinessStructure.EmployeeBaseModel>?> GetEmployeeBusinessUnitTypes(Models.Employees.EmployeeBasicGetModel basic)
    {
        var Data = await DataAccess.Employees.EmployeeBusinessUnitTypesDataAccess.GetEmployeeBusinessUnitTypesByCompany(_sql, basic);
        if (Data == null || Data.Count() == 0) return default;
        return Data.ToList();
    }

    public async Task<ResultsModel?> SaveEmployeeBusinessUnitTypes(Models.Employees.EmployeeBasicGetModel basic, EmployeeBaseModel businessUnit)
    {
        if (businessUnit == null) return new ResultsModel { isValid = false, Message = "Invalid business unit data." };
        var Save = businessUnit.ToEmployeeBusinessUnitTypesSaveModel(basic);
        var result = await DataAccess.Employees.EmployeeBusinessUnitTypesDataAccess.SaveEmployeeBusinessUnitTypes(_sql, Save);
        return result.FirstOrDefault();
    }

    public async Task<ResultsModel?> DeleteEmployeeBusinessUnitTypes(Models.Employees.EmployeeBasicGetModel basic, string uuid)
    {
        if (string.IsNullOrEmpty(uuid)) return new ResultsModel { isValid = false, Message = "Invalid business unit selected." };
        DeleteModel delete = new DeleteModel
        {
            CompanyUUID = basic.CompanyUUID,
            UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
            UUID = uuid
        };

        var result = await DataAccess.Employees.EmployeeBusinessUnitTypesDataAccess.DeleteEmployeeBusinessUnitTypes(_sql, delete);
        return result.FirstOrDefault();
    }

}
