using Library.API.Assess.Services.Jobs;
using Library.API.Assess.Services.Projects;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Assess.Services
{
    public static class BuilderService
    {
        public static void AddAssessServices(this IServiceCollection services)
        {
            services.AddScoped<IJobsService, JobsService>();
            services.AddScoped<ICandidateRegistrationService, CandidateRegistrationService>();
            services.AddScoped<ICandidateLoginService, CandidateLoginService>();
            services.AddScoped<IAssessmentUsersService, AssessmentUsersService>();
            services.AddScoped<IProjectsService, ProjectsService>();
        }
    }
}
