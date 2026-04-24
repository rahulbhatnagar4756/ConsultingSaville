using Library.Base.Mapper.BusinessStructure;
using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Base.Models.Employees.BusinessStructure;
using Library.Database.DAL;

namespace Library.Base.Services.Employees;

public interface IEmployeeBusinessUnitsService
{
    Task<ResultsModel?> DeleteEmployeeBusinessUnit(EmployeeBasicGetModel basic, string uuid);
    Task<List<EmployeeBaseModel>?> GetEmployeeBusinessUnits(EmployeeBasicGetModel basic);
    Task<ResultsModel?> SaveEmployeeBusinessUnit(EmployeeBasicGetModel basic, EmployeeBaseModel businessUnit);
}

public class EmployeeBusinessUnitsService : IEmployeeBusinessUnitsService
{
    private readonly ISqlDataAccess _sql;

    public EmployeeBusinessUnitsService(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public async Task<List<EmployeeBaseModel>?> GetEmployeeBusinessUnits(Models.Employees.EmployeeBasicGetModel basic)
    {
        var Data = await DataAccess.Employees.EmployeeBusinessUnitsDataAccess.GetEmployeeBusinessUnitsByCompany(_sql, basic);
        if (Data == null || Data.Count() == 0) return default;
        return Data.ToList();
    }

    public async Task<ResultsModel?> SaveEmployeeBusinessUnit(Models.Employees.EmployeeBasicGetModel basic, EmployeeBaseModel businessUnit)
    {
        if (businessUnit == null) return new ResultsModel { isValid = false, Message = "Invalid business unit data." };
        var Save = businessUnit.ToEmployeeBusinessUnitsSaveModel(basic);
        var result = await DataAccess.Employees.EmployeeBusinessUnitsDataAccess.SaveEmployeeBusinessUnit(_sql, Save);
        return result.FirstOrDefault();
    }

    public async Task<ResultsModel?> DeleteEmployeeBusinessUnit(Models.Employees.EmployeeBasicGetModel basic, string uuid)
    {
        if (string.IsNullOrEmpty(uuid)) return new ResultsModel { isValid = false, Message = "Invalid business unit selected." };
        DeleteModel delete = new DeleteModel
        {
            CompanyUUID = basic.CompanyUUID,
            UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
            UUID = uuid
        };

        var result = await DataAccess.Employees.EmployeeBusinessUnitsDataAccess.DeleteEmployeeBusinessUnit(_sql, delete);
        return result.FirstOrDefault();
    }

}