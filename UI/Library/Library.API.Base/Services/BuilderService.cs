using Library.API.Base.Services.BusinessHierarchy;
using Library.API.Base.Services.BusinessHierarchy.Jobs;
using Library.API.Base.Services.Users;
using Library.Security.AuthenticationStateProviders;
using Library.Security.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Services
{
    public static class BuilderService
    {
        public static void AddBaseServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IBusinessUnitTypesService, BusinessUnitTypesService>();
            services.AddScoped<IBusinessUnitsService, BusinessUnitsService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUserDataService, UserDataService>();
            services.AddScoped<IUserLoginService, UserLoginService>();
            services.AddScoped<IUserRolesService, UserRolesService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<ILevelsService, LevelsService>();
            services.AddScoped<IJobDisciplinesService, JobDisciplinesService>();
            services.AddScoped<ICriticalRolesService, CriticalRolesService>();
            services.AddScoped<IPositionEducationService, PositionEducationService>();
            services.AddScoped<IPositionExperienceService, PositionExperienceService>();
        }

    }
}
