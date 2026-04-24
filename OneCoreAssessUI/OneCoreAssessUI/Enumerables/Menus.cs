using Microsoft.AspNetCore.Components;
using OneCoreAssessUI.Models;
using OneCoreAssessUI.Services;

namespace OneCoreAssessUI.Enumerables
{
    public enum MenuSectionTypes
    {
        Login,
        Home,
        Assessments,
        Selections,
        LeaderShipAndDevelopment,
        PerformanceManagement,
        Administration,
        Employee,
        Company
    }

    public enum MenuSubSectionTypes
    {
        Home,
        Assessments,
        Selections,
        LeaderShipAndDevelopment,
        PerformanceManagement,
        Administration,
        Employee,
        Company

    }

    public enum MenuItemTypes
    {
        HomeDashboard,
        ProjectDashboard,
        Projects,
        ProjectCandidates,
        ProjectImports,
        ProjectEmployeeReports,
        ProjectTeamReport,
        ProjectReports,
        SelectionDashboard,
        SectionSuccessionPlanning,
        SectionReports,
        LeaderShipAndDevelopmentDashboard,
        LeaderShipAndDevelopmentIDP,
        LeaderShipAndDevelopmentImports,
        LeaderShipAndDevelopmentReports,
        PerformanceManagementDashboard,
        PerformanceManagement360,
        PerformanceManagementGoals,
        PerformanceManagementRobotDashboard,
        PerformanceManagementImports,
        PerformanceManagementReports,
        PerformanceManagementSettings,
        AdministrationDashboard, 
        AdministrationSettings,
        AdministrationBusinessHierarchy ,
        BusinessType,
        BusinessUnit,
        Department,
        Jobs,
        AdministrationTolerances,
        TalentManagement,
        SuccessionReadiness,
        DevelopmentProject,
        EmployeeDashboard,
        ESP, 
        Employees,
        Roles,
        MyTeam,
        MyIDP,
        Goals,
        Assessments,
        CompanyDashboard,
        GoalsAdministration,
        GoalsContracts,
        GoalsContractPeriods,
        GoalsEmployees,
        GoalContract,
       
    }

    public enum MenuItemSubTypes
    {
        Dashboard,
        AdministrationBusinessHierarchyBusinessUnits,
        AdministrationBusinessHierarchyBusinessUnitTypes,
        AdministrationBusinessHierarchyDepartments,
        AdministrationBusinessHierarchyLevels,
        AdministrationBusinessHierarchyPositions,
        AdministrationBusinessHierarchyJobDisciplines,

        AdministrationGoalsTolerances,
        AdministrationGoalsPillars,
        AdministrationGoalsStatus,
        AdministrationGoalsEnterpriseStructureTypes,
        AdministrationGoalsEnterpriseStructureWeights,
        AdministrationGoalsContractPeriods,
        ContractGoalsTemplates,
        ContractGoalsEmployees,
        ContractGoalContract,
        GoalRatingPeriods,
        GoalScoringPeriods,
        EmployeeGoals,
    }

    public class Menus  
    {
        private readonly NavigationManager _navigationManager;
        private readonly LoggedInInformationModel _loggedInInformation;

        public Menus(NavigationManager navigationManager, LoggedInInformationModel loggedInInformation)
        {
            _navigationManager = navigationManager;
            _loggedInInformation = loggedInInformation;
        }

        public void RedirectURLAddress(LayoutCascadeMenuModel section)
        {
            if (_loggedInInformation == null || _loggedInInformation.User == null || _loggedInInformation.Company == null) return;

            //switch (section.MenuSectionId)
            //{
            //    case MenuSectionTypes.Login:
            //        _navigationManager.NavigateTo($"{_loggedInInformation.Company.URLAddress}Admin/Login.aspx");
            //        break;
            //    case MenuSectionTypes.Home:
            //        _navigationManager.NavigateTo($"{_loggedInInformation.Company.URLAddress}Admin/DefaultTS.aspx");
            //        break;
            //    case MenuSectionTypes.Assessments:
            //        _navigationManager.NavigateTo($"{_loggedInInformation.Company.URLAddress}Admin/DefaultTSAssessment.aspx");
            //        break;
            //    case MenuSectionTypes.Selections:
            //        _navigationManager.NavigateTo($"{_loggedInInformation.Company.URLAddress}Admin/DefaultTSelect.aspx");
            //        break;
            //    //case MenuSectionTypes.LeaderShipAndDevelopment:
            //    //    _navigationManager.NavigateTo($"{_loggedInInformation.Company.URLAddress}Admin/DefaultTSDev.aspx");
            //    //    break;
            //    case MenuSectionTypes.PerformanceManagement:
            //        _navigationManager.NavigateTo($"{_loggedInInformation.Company.URLAddress}Admin/DefaultTSManagement.aspx");
            //        break;
            //        //case MenuSectionTypes.Administration:
            //        //    _navigationManager.NavigateTo($"{_loggedInInformation.Company.URLAddress}Admin/DefaultTSAdministration.aspx");
            //        //    break;
            //}

        }


    }
}
