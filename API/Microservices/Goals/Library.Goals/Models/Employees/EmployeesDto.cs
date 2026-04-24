using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.Employees
{
    /// <summary>
    /// Data Transfer Object representing details of an employee.
    /// Used to transfer employee data between application layers.
    /// </summary>
    public class EmployeeDto
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        public string? UsersUUID { get; set; }
        /// <summary>
        /// First name of the employee.
        /// </summary>
        public string? FirstName { get; set; }
        /// <summary>
        /// Last name of the employee.
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// Email address of the employee.
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Names or identifiers of the business units the employee is assigned to.
        /// </summary>
        public string? EmployeeBusinessUnits { get; set; }
        /// <summary>
        /// Types of business units.
        /// </summary>
        public string? EmployeeBusinessUnitTypes { get; set; }
        /// <summary>
        /// Employee's date of birth.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Names or identifiers of the departments the employee is part of.
        /// </summary>
        public string? EmployeeDepartments { get; set; }
        /// <summary>
        /// Mobile phone number of the employee.
        /// </summary>
        public string? Mobile { get; set; }
        /// <summary>
        /// Job titles or roles the employee holds.
        /// This may be a comma-separated list if the employee has multiple roles.
        /// </summary>
        public string? EmployeeJobs { get; set; }
        /// <summary>
        /// Employee levels or grades.
        /// </summary>
        public string? EmployeeLevels { get; set; }
        /// <summary>
        /// Full name of the employee's manager.
        /// </summary>
        public string? FullNameManager { get; set; }
        /// <summary>
        /// Indicates whether the employee has a valid contract.
        /// </summary>
        public bool IsValidContract { get; set; }
    }


    
}
