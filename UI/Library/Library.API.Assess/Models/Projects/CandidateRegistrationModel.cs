using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Assess.Models.Projects
{
    public class CandidateRegistrationModel
    {
        public string? CompanyUUID { get; set; }
        public string? ProjectsUUID { get; set; }
        public string? IDNumber { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string? Email { get; set; }

        [StringLength(50, ErrorMessage = "Mobile number cannot exceed 50 characters")]
        public string? Mobile { get; set; }

        public string? Gender { get; set; }
        public string? Ethnicity { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
