namespace OneCoreAssessUI.Enumerables
{
    /// <summary>
    /// format for type WhereTheControlGoes_Section_Action
    /// WhereTheControlGoes, sb - sidebar, a - anywhere, c - content
    /// Section - Employee, Admin, IDP, ...
    /// Action - Filter, Search, ...
    /// </summary>
    public enum ControlTypes
    {
        none = 0,
        c_employee_search = 1,
        sb_employee_filter = 2,
        sb_9Box_Employee_levels,
        sb_Succession_Planning,
        sb_Administration_BusinessHierarchy_BusinessUnits,
        sb_Administration_BusinessHierarchy_BusinessUnitTypes,
        sb_Administration_BusinessHierarchy_Department,
        sb_Administration_BusinessHierarchy_Positions,
        sb_Administration_BusinessHierarchy_Levels,
        sb_Administration_BusinessHierarchy_JobDisciplines,
        sb_Goals_Administration_ToleranceSets,
        sb_Goals_Administration_Pillars,
        sb_Goals_Administration_Status,
        sb_Goals_Administration_EnterpriseStructureTypes,
        sb_Goals_Administration_EnterpriseStructureWeights,

        sb_Candidate_TestInstructions,
        sb_Candidate_Profile,

        sb_Goals_Contracts,
        sb_Goals_Contracts_Templates,
        sb_Goals_Contracts_AssignTemplates,
        sb_Goals_Administration_ContractPeriods,
 
        sb_Goals_Employees,
        sb_Goals_Contracts_KPA,
        sb_Goals_Contracts_KPI,
        sb_Goals_Contracts_Linked,
        sb_Goals_RatingPeriods,
        sb_Goals_AddRatingPeriod,
        sb_Goals_ScoringPeriods,
        sb_Goals_EmployeeGoals,
        sb_Goals_GlobalComments
    }



    public static class ControlEnumerableExtras
    {
        public static string GetControlTypeWidth(this ControlTypes control)
        {
            var statusDescriptions = new Dictionary<ControlTypes, string>
            {
                { ControlTypes.none, "600px" },
                { ControlTypes.c_employee_search, "50%" },
                { ControlTypes.sb_employee_filter, "800px" },
                { ControlTypes.sb_9Box_Employee_levels, "800px" },
                { ControlTypes.sb_Succession_Planning, "800px" },   
                { ControlTypes.sb_Administration_BusinessHierarchy_BusinessUnits, "800px" },
                { ControlTypes.sb_Administration_BusinessHierarchy_BusinessUnitTypes, "800px" },
                { ControlTypes.sb_Administration_BusinessHierarchy_Department, "800px" },
                { ControlTypes.sb_Administration_BusinessHierarchy_Positions, "800px" },
                { ControlTypes.sb_Administration_BusinessHierarchy_Levels, "800px" },
                { ControlTypes.sb_Administration_BusinessHierarchy_JobDisciplines, "800px" },
                { ControlTypes.sb_Goals_Administration_ToleranceSets, "800px" },
                { ControlTypes.sb_Goals_Administration_Pillars, "800px" },
                { ControlTypes.sb_Goals_Administration_Status, "1000px" },
                { ControlTypes.sb_Goals_Administration_EnterpriseStructureTypes, "800px" },
                { ControlTypes.sb_Goals_Administration_EnterpriseStructureWeights, "800px" },

                { ControlTypes.sb_Candidate_TestInstructions, "800px" },
                { ControlTypes.sb_Candidate_Profile, "800px" },

                { ControlTypes.sb_Goals_Contracts_Templates, "800px" },
                {ControlTypes.sb_Goals_Employees,"850px" },

                 {ControlTypes.sb_Goals_RatingPeriods,"600px" },
                 {ControlTypes.sb_Goals_Contracts_Linked,"600px" },
            };

            return statusDescriptions.GetValueOrDefault(control, "600px");
        }
    }
}
