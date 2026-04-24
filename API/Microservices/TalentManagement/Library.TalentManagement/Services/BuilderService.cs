using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TalentManagement.Services;

public static class BuilderService
{
    public static void AddTalentManagementServices(this IServiceCollection services)
    {
        services.AddScoped<IBox9Service, Box9Service>();
    }
}
