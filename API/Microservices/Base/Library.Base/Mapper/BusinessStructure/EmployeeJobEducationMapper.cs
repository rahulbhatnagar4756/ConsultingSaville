using Library.Base.Models.Employees;
using Library.Base.Models.Employees.Jobs;

namespace Library.Base.Mapper.BusinessStructure;

public static class EmployeeJobEducationMapper
{
    /// <summary>
    /// Map EmployeeJobEducationBaseModel to EmployeeJobEducationSaveModel
    /// </summary>
    /// <param name="model"></param>
    /// <param name="basic"></param>
    /// <returns></returns>
    public static EmployeeJobEducationSaveModel ToEmployeeJobEducationSaveModel(this EmployeeJobEducationBaseModel model, EmployeeBasicGetModel basic)
    {
        return new EmployeeJobEducationSaveModel
        {
            CompanyUUID = basic.CompanyUUID,
            UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
            UUID = model.UUID,
            EmployeeJobsUUID = model.EmployeeJobsUUID,
            Information = model.Information,
            OrderVal = model.OrderVal
        };
    }
}
