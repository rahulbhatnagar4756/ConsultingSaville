using Library.API.Base.Enumerables;
using Library.API.Base.Models.BusinessHierarchy.EmployeeCriteria;
using Library.API.Base.Models.BusinessHierarchy.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Builder.Employee;

internal class FilterCriteriaBuilder
{
    public EmployeeCriteriaResultsModel? FilterModel(EmployeeFilterTypes filterTypes, EmployeeCriteriaResultsModel filterResults)
    {
           if(filterResults == null) return filterResults;
           
           EmployeeCriteriaResultsModel live = filterResults;
           List<Models.TypesModel> selected;

        switch (filterTypes)
        {
            case EmployeeFilterTypes.BusinessUnitTypes:
                if(filterResults.BusinessUnitTypes == null) return filterResults;
                live.BusinessUnitTypes = filterResults.BusinessUnitTypes;
                selected = filterResults.BusinessUnitTypes.Where(x => x.isSelected == true).ToList();
                if (selected.Count() == 0) selected = filterResults.BusinessUnitTypes;
                live.BusinessUnits = filterResults.BusinessUnits.Where(x => selected.Any(y => y.UUID == x.TrackingUUID)).ToList();

                goto case EmployeeFilterTypes.BusinessUnits;

            case EmployeeFilterTypes.BusinessUnits:
                live.BusinessUnits = filterResults.BusinessUnits;
                selected = filterResults.BusinessUnits.Where(x => x.isSelected == true).ToList();
                if (selected.Count() == 0) selected = filterResults.BusinessUnits;
                live.Departments = filterResults.Departments.Where(x => selected.Any(y => y.UUID == x.TrackingUUID)).ToList();
                break;
        }

        List<Models.TypesModel> Departments = live.Departments.Where(x => x.isSelected == true).ToList();
        if (Departments.Count() == 0) Departments = live.Departments;
        List<Models.TypesModel> Levels = live.Level.Where(x => x.isSelected == true).ToList();
        List<Models.TypesModel> CriticalRoles = live.CriticalRoles.Where(x => x.isSelected == true).ToList();
        List<Models.TypesModel> Disciplines = live.Disciplines.Where(x => x.isSelected == true).ToList();


        List<EmployeeJobsModel> Positions = live.Position.Where(x => Departments.Any(y => y.UUID == x.EmployeeDepartmentsUUID)
                                                                                   && (!Levels.Any() || Levels.Any(y => y.UUID == x.EmployeeLevelsUUID))
                                                                                   && (!CriticalRoles.Any() || CriticalRoles.Any(y => y.UUID == x.EmployeeJobsCriticalRolesUUID))
                                                                                   && (!Disciplines.Any() || Disciplines.Any(y => y.UUID == x.EmployeeJobDisciplinesUUID))
        ).ToList();

        live.Position = Positions;

        return live;
    }
}
