using Library.API.TalentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.TalentManagement.Mapper
{
    internal static class SuccessionPlanningMapper
    {
        public static DataTable MapSuccessionPlanningToDataTable(this List<SuccessionPlanningRawModel>? successionPlannings)
        {

            DataTable Table = new DataTable("SuccessionPlanning");
            Table.Columns.Add("JobName", typeof(string));
            Table.Columns.Add("JobLevel", typeof(string));
            Table.Columns.Add("FirstName", typeof(string));
            Table.Columns.Add("LastName", typeof(string));
            Table.Columns.Add("Email", typeof(string));
            Table.Columns.Add("IDNumber", typeof(string));
            Table.Columns.Add("EmployeeNumber", typeof(string));
            Table.Columns.Add("Gender", typeof(string));
            Table.Columns.Add("Ethnicity", typeof(string));
            Table.Columns.Add("BusinessUnits", typeof(string));
            Table.Columns.Add("Departments", typeof(string));
            Table.Columns.Add("Jobs", typeof(string));
            Table.Columns.Add("Level", typeof(string));
            Table.Columns.Add("Readiness", typeof(string));
            Table.Columns.Add("YearsOfExperienceYear", typeof(int));
            Table.Columns.Add("YearsOfExperienceMonth", typeof(int));

            if (successionPlannings == null || successionPlannings.Count() == 0) return Table;

            foreach (var item in successionPlannings)
            {
                Table.Rows.Add(
                    item.JobName,
                    item.Level,
                    item.FirstName,
                    item.LastName,
                    item.Email,
                    item.IDNumber,
                    item.EmployeeNumber,
                    item.Gender,
                    item.Race,
                    item.EmployeeBusinessUnits,
                    item.EmployeeDepartments,
                    item.EmployeeJobs,
                    item.EmployeeLevel,
                    item.Readiness,
                    item.YearsOfExperienceYear ?? null,
                    item.YearsOfExperienceMonth ?? null
                );
            }

            return Table;
        }


    }
}
