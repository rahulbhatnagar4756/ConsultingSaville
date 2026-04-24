using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.Employee
{
    /// <summary>
    /// Organizational and structural data related to an employee.
    /// Includes business units, departments, roles, levels, and reporting manager.
    /// </summary>
    public class BusinessData
    {
        /// <summary>
        /// The types of business units the employee.
        /// </summary>
        public string BusinessUnitTypes { get; set; }
        /// <summary>
        /// The names or identifiers of the business units the employee belongs to.
        /// </summary>
        public string BusinessUnits { get; set; }
        /// <summary>
        /// The departments the employee is associated with (e.g., IT, HR, Finance).
        /// </summary>
        public string Departments { get; set; }
        /// <summary>
        /// The job roles or titles held by the employee (e.g., Software Engineer, Project Manager).
        /// </summary>
        public string Jobs { get; set; }
        /// <summary>
        /// The level or grade of the employee in the organization (e.g., Junior, Mid, Senior, Executive).
        /// </summary>
        public string Levels { get; set; }
        /// <summary>
        /// The full name of the employee's reporting manager or supervisor.
        /// </summary>
        public string FullNameManager { get; set; }
    }

    /// <summary>
    /// Model for capturing new business position form input.
    /// </summary>
    public class NewBusinessPositionModel
    {
        /// <summary>
        /// UUID of the selected business unit.
        /// </summary>
        public string? EmployeeBusinessUnitsUUID { get; set; }
        /// <summary>
        /// UUID of the selected department.
        /// </summary>
        public string? EmployeeDepartmentsUUID { get; set; }
        /// <summary>
        /// UUID of the selected job/position.
        /// </summary>
        public string? EmployeeJobsUUID { get; set; }
        /// <summary>
        /// UUID of the manager user.
        /// </summary>
        public string? UsersUUIDManager { get; set; }
        /// <summary>
        /// UUID of the business position record.
        /// </summary>
        public string? UUID { get; set; }
    }
}
