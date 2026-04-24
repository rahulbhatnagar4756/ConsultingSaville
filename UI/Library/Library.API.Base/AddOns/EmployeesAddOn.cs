using Library.API.Base.Models;
using Library.API.Base.Models.BusinessHierarchy.EmployeeCriteria;
using Library.API.Base.Models.BusinessHierarchy.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.AddOns
{
    public static class EmployeesAddOn
    {
        public static List<TypesModel>? Clone(this List<TypesModel> types) =>
            types?.Select(l => (TypesModel)l.Clone())?.ToList();

        public static List<EmployeeJobsModel>? Clone(this List<EmployeeJobsModel> jobs) =>
            jobs?.Select(l => (EmployeeJobsModel)l.Clone())?.ToList();

       
        public static Models.Employees.EmployeeSearchModel ConvertToEmployeeSearchModel (this EmployeeCriteriaResultsModel resultsModel) {
            Models.Employees.EmployeeSearchModel searchModel = new(); 
            Mappers.EmployeesMapper.EmployeeCriteriaResultsModelToEmployeeSearchDTO(resultsModel, searchModel);
            return searchModel;
        }

    }
}
