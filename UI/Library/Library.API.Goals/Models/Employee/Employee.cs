using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.Employee
{
    /// <summary>
    /// Represents an employee entity in the Goals module.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Gets or sets the unique user UUID of the employee.
        /// </summary>
        public string UsersUUID { get; set; }

        /// <summary>
        /// Gets or sets the first name of the employee.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the employee.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        public string Fullname => FirstName + " " + LastName;

        /// <summary>
        /// Gets or sets the email of the employee.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the business unit of the employee.
        /// </summary>
        public string? EmployeeBusinessUnits { get; set; }

        /// <summary>
        /// Gets or sets the department of the employee.
        /// </summary>
        public string? EmployeeDepartments { get; set; }

        /// <summary>
        /// Gets or sets the job title of the employee.
        /// </summary>
        public string? EmployeeJobs { get; set; }

        /// <summary>
        /// Gets or sets the job level of the employee.
        /// </summary>
        public string? EmployeeLevels { get; set; }

        /// <summary>
        /// Gets or sets the contract validity status.
        /// </summary>
        public bool IsContractValid { get; set; }

        public string? EmployeeBusinessUnitTypes { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Mobile { get; set; }
        public string? FullNameManager { get; set; }
    }
}
