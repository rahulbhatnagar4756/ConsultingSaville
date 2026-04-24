using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.Services
{
    public static class BuilderService
    {
        public static void AddFrogServices(this IServiceCollection services)
        {
            services.AddScoped<IFrogService, FrogService>();
            services.AddScoped<IFrogDataLogsService, FrogDataLogsService>();
        }

    }
}
