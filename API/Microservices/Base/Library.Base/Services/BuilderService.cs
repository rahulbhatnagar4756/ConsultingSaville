using Library.Base.Services.Admin;
using Library.Base.Services.Employees;
using Library.Base.Services.Employees.Jobs;
using Library.Database.DAL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Services
{
    public static class BuilderService
    {

        public static void AddAdminServices(this IServiceCollection services)
        {
            services.AddScoped<IIconsService, IconsService>(); 
        }

        public static void AddEmployeeServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeBusinessUnitTypesService, EmployeeBusinessUnitTypesService>();
            services.AddScoped<IEmployeeBusinessUnitsService, EmployeeBusinessUnitsService>();
            services.AddScoped<IEmployeeDepartmentsService, EmployeeDepartmentsService>();
            services.AddScoped<IEmployeeJobsService, EmployeeJobsService>();
            services.AddScoped<IEmployeeLevelsService, EmployeeLevelsService>();
            services.AddScoped<IEmployeeJobDisciplinesService, EmployeeJobDisciplinesService>();
            services.AddScoped<IEmployeeJobsCriticalRolesService, EmployeeJobsCriticalRolesService>();
            services.AddScoped<IEmployeeJobEducationService, EmployeeJobEducationService>();
            services.AddScoped<IEmployeeJobExperienceService, EmployeeJobExperienceService>();
            services.AddScoped<IEmployeeHierarchyService, EmployeeHierarchyService>();
        }

        public static void AddUsersServices(this IServiceCollection services)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUserRolesService, UserRolesService>();
            services.AddScoped <IUserDataService, UserDataService>();
            services.AddScoped <IUserLoginService, UserLoginService>();
        }

        public static void AddCompanyServices(this IServiceCollection services)
        {
            services.AddScoped<ICompanyService, CompanyService>();
        }

    }
}
