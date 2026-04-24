using Microsoft.AspNetCore.Components;
using OneCoreAssessUI.Enumerables;
using OneCoreAssessUI.Services;

namespace OneCoreAssessUI.Models
{
    public class LayoutCascadeMenuModel
    {
        //create an event when the MenuItemSubTypes
        public event Action<Enumerables.MenuItemSubTypes> OnMenuItemSubTypeChanged = delegate { };



        private Enumerables.MenuSectionTypes _menuSectionId = Enumerables.MenuSectionTypes.Home;
        private Enumerables.MenuSubSectionTypes _menuSubSectionId = Enumerables.MenuSubSectionTypes.Home;
        private Enumerables.MenuItemTypes _menuItemType = Enumerables.MenuItemTypes.HomeDashboard;
        private Enumerables.MenuItemSubTypes _menuItemSubType = Enumerables.MenuItemSubTypes.Dashboard;
        private Enumerables.MenuItemSubTypes? _menuItemSubTypeCurrent = Enumerables.MenuItemSubTypes.Dashboard;
        private readonly Menus _menus;

        public Enumerables.MenuSectionTypes MenuSectionId { get { return _menuSectionId; } set { SelectMenuSectionTypes(value); } }
        public Enumerables.MenuSubSectionTypes MenuSubSectionId { get { return _menuSubSectionId; } set { SelectMenuSubSectionTypes(value); } }
        public Enumerables.MenuItemTypes MenuItemType { get { return _menuItemType; } set { SelectMenuItemTypes(value); } }
        public Enumerables.MenuItemSubTypes MenuItemSubType { get { return _menuItemSubType; } set { SelectMenuItemSubTypes(value); } }
        public bool isMasterDashboard {get; set;} = true;

        public object? SharedContext { get; set; }

        public T? GetSharedContext<T>() where T : notnull
        {
            if (SharedContext is T typedContext)
                return typedContext;

            return default;
        }

        /// <summary>
        /// Set the shared context for cross-component communication
        /// </summary>
        /// <param name="context">The context object to share</param>
        public void SetSharedContext(object context)
        {
            SharedContext = context;
        }


        private Dictionary<Enumerables.MenuSectionTypes, Enumerables.MenuSubSectionTypes> _defaultSubSection = new Dictionary<Enumerables.MenuSectionTypes, Enumerables.MenuSubSectionTypes>
        {
            {Enumerables.MenuSectionTypes.Home, Enumerables.MenuSubSectionTypes.Home},
            {Enumerables.MenuSectionTypes.Assessments, Enumerables.MenuSubSectionTypes.Assessments},
            {Enumerables.MenuSectionTypes.Selections, Enumerables.MenuSubSectionTypes.Selections},
            {Enumerables.MenuSectionTypes.LeaderShipAndDevelopment, Enumerables.MenuSubSectionTypes.LeaderShipAndDevelopment},
            {Enumerables.MenuSectionTypes.PerformanceManagement, Enumerables.MenuSubSectionTypes.PerformanceManagement},
            {Enumerables.MenuSectionTypes.Administration, Enumerables.MenuSubSectionTypes.Administration},
            {Enumerables.MenuSectionTypes.Employee, Enumerables.MenuSubSectionTypes.Employee},
            {Enumerables.MenuSectionTypes.Company, Enumerables.MenuSubSectionTypes.Company}
        };
        
        private Dictionary<Enumerables.MenuSubSectionTypes, Enumerables.MenuItemTypes> _defaultItemSection = new Dictionary<Enumerables.MenuSubSectionTypes, Enumerables.MenuItemTypes>
        {
            {  Enumerables.MenuSubSectionTypes.Home, Enumerables.MenuItemTypes.HomeDashboard},
            {  Enumerables.MenuSubSectionTypes.Assessments, Enumerables.MenuItemTypes.ProjectDashboard},
            {  Enumerables.MenuSubSectionTypes.Selections, Enumerables.MenuItemTypes.SelectionDashboard},
            {  Enumerables.MenuSubSectionTypes.LeaderShipAndDevelopment, Enumerables.MenuItemTypes.LeaderShipAndDevelopmentDashboard},
            {  Enumerables.MenuSubSectionTypes.PerformanceManagement, Enumerables.MenuItemTypes.PerformanceManagementDashboard},
            {  Enumerables.MenuSubSectionTypes.Administration, Enumerables.MenuItemTypes.AdministrationDashboard},
            {  Enumerables.MenuSubSectionTypes.Employee, Enumerables.MenuItemTypes.EmployeeDashboard},
            {  Enumerables.MenuSubSectionTypes.Company, Enumerables.MenuItemTypes.CompanyDashboard}
        };

        private Dictionary<Enumerables.MenuItemSubTypes, Enumerables.MenuItemTypes> _defaultItemSubSection = new Dictionary<Enumerables.MenuItemSubTypes, Enumerables.MenuItemTypes>
        {
            {Enumerables.MenuItemSubTypes.AdministrationBusinessHierarchyBusinessUnitTypes, Enumerables.MenuItemTypes.AdministrationBusinessHierarchy},
            {Enumerables.MenuItemSubTypes.AdministrationBusinessHierarchyBusinessUnits, Enumerables.MenuItemTypes.AdministrationBusinessHierarchy},
            {Enumerables.MenuItemSubTypes.AdministrationBusinessHierarchyDepartments, Enumerables.MenuItemTypes.AdministrationBusinessHierarchy},
            {Enumerables.MenuItemSubTypes.AdministrationBusinessHierarchyPositions, Enumerables.MenuItemTypes.AdministrationBusinessHierarchy},
            {Enumerables.MenuItemSubTypes.AdministrationBusinessHierarchyLevels, Enumerables.MenuItemTypes.AdministrationBusinessHierarchy},
            {Enumerables.MenuItemSubTypes.AdministrationBusinessHierarchyJobDisciplines, Enumerables.MenuItemTypes.AdministrationBusinessHierarchy},

            {Enumerables.MenuItemSubTypes.AdministrationGoalsTolerances, Enumerables.MenuItemTypes.GoalsAdministration},
            {Enumerables.MenuItemSubTypes.AdministrationGoalsPillars, Enumerables.MenuItemTypes.GoalsAdministration},
            {Enumerables.MenuItemSubTypes.AdministrationGoalsStatus, Enumerables.MenuItemTypes.GoalsAdministration},
            {Enumerables.MenuItemSubTypes.AdministrationGoalsEnterpriseStructureTypes, Enumerables.MenuItemTypes.GoalsAdministration},
            {Enumerables.MenuItemSubTypes.AdministrationGoalsEnterpriseStructureWeights, Enumerables.MenuItemTypes.GoalsAdministration},
            {Enumerables.MenuItemSubTypes.AdministrationGoalsContractPeriods, Enumerables.MenuItemTypes.GoalsAdministration},
               {Enumerables.MenuItemSubTypes.GoalRatingPeriods, Enumerables.MenuItemTypes.GoalsAdministration},
                {Enumerables.MenuItemSubTypes.GoalScoringPeriods, Enumerables.MenuItemTypes.GoalsAdministration},
            {Enumerables.MenuItemSubTypes.ContractGoalsTemplates, Enumerables.MenuItemTypes.GoalsContracts},
            {Enumerables.MenuItemSubTypes.ContractGoalsEmployees, Enumerables.MenuItemTypes.GoalsContracts},
            {Enumerables.MenuItemSubTypes.ContractGoalContract, Enumerables.MenuItemTypes.GoalsContracts},
                {Enumerables.MenuItemSubTypes.EmployeeGoals, Enumerables.MenuItemTypes.GoalsAdministration},
        };
        
         
        public LayoutCascadeMenuModel(NavigationManager navigationManager, LoggedInInformationModel infoLoggedIn)
        {
            _menus = new Menus(navigationManager, infoLoggedIn);
        }

        private void SelectMenuSectionTypes(Enumerables.MenuSectionTypes selection)
        {
            _menuSectionId = selection;
            Enumerables.MenuSubSectionTypes data;
            if (!_defaultSubSection.TryGetValue(selection, out data)) return;
            SelectMenuSubSectionTypes(data);
        }

        private void SelectMenuSubSectionTypes(Enumerables.MenuSubSectionTypes sub)
        {
            _menuSubSectionId = sub;
            Enumerables.MenuItemTypes data;
            if (!_defaultItemSection.TryGetValue(sub, out data)) return;
            SelectMenuItemTypes(data);
        }

        private void SelectMenuItemTypes(Enumerables.MenuItemTypes item)
        {
            _menuItemType = item;
            _menus.RedirectURLAddress(this);
            isMasterDashboard = true;
        }

        private void SelectMenuItemSubTypes(Enumerables.MenuItemSubTypes item)
        {
            _menuItemSubType = item;
            Enumerables.MenuItemTypes data;
            _defaultItemSubSection.TryGetValue(item, out data);
            SelectMenuItemTypes(data);

            if(_menuItemSubTypeCurrent == null || _menuItemSubTypeCurrent != _menuItemSubType)
            {
                _menuItemSubTypeCurrent = item;
                OnMenuItemSubTypeChanged?.Invoke(item);
            }
        }

        /// <summary>
        /// this is used to set that the master dashboard for a specific Item does not need to be used
        /// </summary>
        public void SetMasterDashboardOff() =>
            isMasterDashboard = false;
    }
}
