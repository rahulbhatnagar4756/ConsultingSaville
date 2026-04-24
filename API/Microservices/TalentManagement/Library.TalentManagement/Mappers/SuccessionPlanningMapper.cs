using Library.TalentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TalentManagement.Mappers;

internal static class SuccessionPlanningMapper
{

    public static List<SuccessionPlanningGenericModel> MapSuccessionPlanningToSuccessionPlanningGeneric(List<SuccessionPlanningModel>? successionPlannings)
    {
        //tetst cout successionPlannings
        if (successionPlannings == null || successionPlannings.Count() == 0) return new List<SuccessionPlanningGenericModel>();

        //get the distinct jobname, level and EmployeeJobsGenericNamesUUID
        var successionPlanningGeneric = successionPlannings
        .GroupBy(x => new { x.EmployeeJobsGenericNamesUUID, x.JobName, x.Level })
        .Select(g => new SuccessionPlanningGenericModel
        {
            EmployeeJobsGenericNamesUUID = g.Key.EmployeeJobsGenericNamesUUID,
            JobName = g.Key.JobName,
            Level = g.Key.Level
        })
        .ToList();

        //get the users for each jobname, level and EmployeeJobsGenericNamesUUID
        foreach (var item in successionPlanningGeneric)
        {
            item.Users = successionPlannings
                .Where(x => x.EmployeeJobsGenericNamesUUID == item.EmployeeJobsGenericNamesUUID && x.JobName == item.JobName && x.Level == item.Level)
                .Select(x => new SuccessionPlanningUserModel
                {
                    UsersUUID = x.UsersUUID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    IDNumber = x.IDNumber,
                    EmployeeNumber = x.EmployeeNumber,
                    Gender = x.Gender,
                    Race = x.Race,
                    EmployeeBusinessUnitTypes = x.EmployeeBusinessUnitTypes,
                    EmployeeBusinessUnits = x.EmployeeBusinessUnits,
                    EmployeeDepartments = x.EmployeeDepartments,
                    EmployeeJobs = x.EmployeeJobs,
                    EmployeeJobsName = x.EmployeeJobsName,
                    EmployeeLevel = x.EmployeeLevel,
                    FrogsUUID = x.FrogsUUID,
                    FrogElementsUUID = x.FrogElementsUUID,
                    ItemUUIDReadiness = x.ItemUUIDReadiness,
                    Readiness = x.Readiness,
                    YearsOfExperienceYear = x.YearsOfExperienceYear,
                    YearsOfExperienceMonth = x.YearsOfExperienceMonth,
                    Box9Score = x.Box9Score,
                    Box9 = x.Box9

                }).ToList();
        }

        return successionPlanningGeneric;
    }

    public static DataTable MapSuccessionPlanningToDatatable(List<SuccessionPlanningModel>? successionPlannings)
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