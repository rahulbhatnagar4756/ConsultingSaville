using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models.BusinessHierarchy
{
    public class PositionsModel : ICloneable
    {
        public string UUID { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public string? Level { get; set; }
        public string? Description { get; set; }
        public string? EmployeeBusinessUnitTypesUUID { get; set; }
        public string? EmployeeBusinessUnitTypes { get; set; }
        public string? EmployeeBusinessUnitsUUID { get; set; }
        public string? EmployeeBusinessUnits { get; set; }
        public string? EmployeeDepartmentsUUID { get; set; }
        public string? EmployeeDepartments { get; set; }
        public int? DisciplineId { get; set; }
        public string? EmployeeJobDisciplinesUUID { get; set; }
        public string? EmployeeJobDisciplines { get; set; }
        public string? EmployeeJobsCriticalRolesUUID { get; set; }
        public string? EmployeeJobsCriticalRoles { get; set; }
        public string? EmployeeLevelsUUID { get; set; }
        public string? JobsUUID { get; set; }
        public string? JobsName { get; set; }
        public int OrderVal { get; set; }

        //clone the parameters
        public object Clone()
        {
            return new PositionsModel
            {
                UUID = this.UUID,
                Name = this.Name,
                Code = this.Code,
                Level = this.Level,
                Description = this.Description,
                EmployeeBusinessUnitTypesUUID = this.EmployeeBusinessUnitTypesUUID,
                EmployeeBusinessUnitTypes = this.EmployeeBusinessUnitTypes,
                EmployeeBusinessUnitsUUID = this.EmployeeBusinessUnitsUUID,
                EmployeeBusinessUnits = this.EmployeeBusinessUnits,
                EmployeeDepartmentsUUID = this.EmployeeDepartmentsUUID,
                EmployeeDepartments = this.EmployeeDepartments,
                DisciplineId = this.DisciplineId,
                EmployeeJobDisciplinesUUID = this.EmployeeJobDisciplinesUUID,
                EmployeeJobDisciplines = this.EmployeeJobDisciplines,
                EmployeeJobsCriticalRolesUUID = this.EmployeeJobsCriticalRolesUUID,
                EmployeeJobsCriticalRoles = this.EmployeeJobsCriticalRoles,
                EmployeeLevelsUUID = this.EmployeeLevelsUUID,
                JobsUUID = this.JobsUUID,
                JobsName = this.JobsName,
                OrderVal = this.OrderVal
            };
        }
    }
}
