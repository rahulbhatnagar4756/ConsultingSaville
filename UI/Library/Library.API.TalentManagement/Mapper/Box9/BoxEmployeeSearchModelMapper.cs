using Library.API.TalentManagement.Models.Box9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.TalentManagement.Mapper.Box9
{
    public static class Box9EmployeeSearchModelMapper
    {
        /// <summary>
        /// map the employee model to box9 employees model
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public static List<Library.API.TalentManagement.Models.Box9.Box9EmployeesModel>? MapperEmployeeModelToBox9Employees(this List<Library.API.Base.Models.Employees.EmployeesModel> employees)
        {
            //in EmployeesModel using the UsersUUID get the first instance
            var uniqueEmployees = employees.GroupBy(e => e.UsersUUID)
                                           .Select(g => g.First())
                                           .ToList();


            //collect the employee information group by box9Score and box9Name and get the select for box9Score and box9Name
            var box9Group = uniqueEmployees.GroupBy(x => new {x.EmployeeLevels, x.Box9Name, x.LevelOrder})
                                           .Select(x => new Box9EmployeesModel { Level = x.Key.EmployeeLevels??"N/A",
                                                                                 Box9 = x.Key.Box9Name,
                                                                                 Order = x.Key.LevelOrder,
                                                                                 Employees = x.ToList()}).ToList();

            return box9Group;
        }


    }
}
