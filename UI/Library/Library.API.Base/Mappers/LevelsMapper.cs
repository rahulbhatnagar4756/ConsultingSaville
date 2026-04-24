using Library.API.Base.Models.BusinessHierarchy;
using Library.API.Base.Models.BusinessHierarchy.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Mappers
{
    public static class LevelsMapper
    {

       public static List<EmployeeBaseModel>? ToEmployeeBaseModel(this List<EmployeeLevelsBaseModel> levels)
       {
            if (levels == null) return default;

            // Convert EmployeeLevelsBaseModel to EmployeeBaseModel format
            return levels.Select(level => new EmployeeBaseModel
            {
                UUID = level.UUID,
                Name = level.Name,
                Description = level.Description,
                Icon = "stat_3", // Always use stat_3 icon as requested
                IconColor = "#362f21", // Default color
                OrderVal = level.LevelOrder,
                Count = 0 // Could be populated with employee count if needed
            })?.ToList() ??default;
       }

        public static EmployeeBaseModel? ToEmployeeBaseModel(this EmployeeLevelsBaseModel level)
        {
            return level == null ? null : new EmployeeBaseModel
            {
                UUID = level.UUID,
                Name = level.Name,
                Description = level.Description,
                Icon = "stat_3", // Always use stat_3 icon as requested
                IconColor = "#362f21", // Default color
                OrderVal = level.LevelOrder,
                Count = 0 // Could be populated with employee count if needed
            };
        }

        public static EmployeeLevelsBaseModel? ToEmployeeLevelsBaseModel(this EmployeeBaseModel ebase)
        {
            return new EmployeeLevelsBaseModel()
            {
                UUID = ebase.UUID,
                Name = ebase.Name,
                Description = ebase.Description,
                LevelOrder = ebase.OrderVal.HasValue ? ebase.OrderVal.Value : 200
            };

        }
    }
}
