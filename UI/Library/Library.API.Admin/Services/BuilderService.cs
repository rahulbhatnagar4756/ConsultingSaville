using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Admin.Services;

public static class BuilderService
{
    public static void AddAdminServices(this IServiceCollection services)
    {
        services.AddScoped<IIconsService, IconsService>(); 
    }
}
