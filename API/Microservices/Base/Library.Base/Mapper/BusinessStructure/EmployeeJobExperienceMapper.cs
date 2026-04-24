using Library.Base.Models.Employees;
using Library.Base.Models.Employees.Jobs;

namespace Library.Base.Mapper.BusinessStructure;

public static class EmployeeJobExperienceMapper
{
    /// <summary>
    /// Map EmployeeJobExperienceBaseModel to EmployeeJobExperienceSaveModel
    /// </summary>
    /// <param name="model"></param>
    /// <param name="basic"></param>
    /// <returns></returns>
    public static EmployeeJobExperienceSaveModel ToEmployeeJobExperienceSaveModel(this EmployeeJobExperienceBaseModel model, EmployeeBasicGetModel basic)
    {
        return new EmployeeJobExperienceSaveModel
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
