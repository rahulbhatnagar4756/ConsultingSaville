using Library.API.Base.Models.BusinessHierarchy;
using Library.API.Base.Models.BusinessHierarchy.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Mappers
{
    public static class JobDisciplineMapper
    {
        /// <summary>
        /// Maps a JobDisciplineBaseModel to an EmployeeJobDisciplineBaseModel
        /// </summary>
        /// <param name="jobDiscipline"></param>
        /// <returns></returns>
        public static EmployeeJobDisciplineBaseModel ToEmployeeJobDisciplineBaseModel(this EmployeeBaseModel? jobDiscipline)
        {
            if (jobDiscipline == null) return default;
            // Convert EmployeeBaseModel to EmployeeJobDisciplineBaseModel format
           
            return new EmployeeJobDisciplineBaseModel
            {
                UUID = jobDiscipline.UUID,
                Name = jobDiscipline.Name,
            };
        }

        public static EmployeeBaseModel ToEmployeeBaseModel(this EmployeeJobDisciplineBaseModel? jobDiscipline)
        {
            if (jobDiscipline == null) return default;
            // Convert EmployeeBaseModel to EmployeeJobDisciplineBaseModel format

            return new EmployeeBaseModel
            {
                UUID = jobDiscipline.UUID,
                Name = jobDiscipline.Name,
                Icon = "person_play", // Always use stat_3 icon as requested
                IconColor = "#362f21", // Default color
                OrderVal = 200, // Default order value
                Count = 0 // Could be populated with employee count if needed
            };
        }

        public static List<EmployeeBaseModel> ToEmployeeBaseModel(this List<EmployeeJobDisciplineBaseModel>? jobDiscipline)
        {
            return jobDiscipline?.Select(discipline => discipline.ToEmployeeBaseModel()).ToList() ?? default;
        }

    }
}
