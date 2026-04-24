using Library.API.Goals.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Services;

public static class BuilderService
{

    public static void AddGoalServices(this IServiceCollection services)
    {
        services.AddScoped<ITemplatesService, TemplatesService>();
        services.AddScoped<IContractsService, ContractsService>();
        services.AddScoped<ITolerancesServices, TolerancesServices>();
        services.AddScoped<IPillarsServices, PillarsServices>();
        services.AddScoped<IStatusService, StatusService>();
        services.AddScoped<IEnterpriseStructuresServices, EnterpriseStructuresServices>();
        services.AddScoped<IContractsDataService, ContractsDataServices>();
        services.AddScoped<IContractPeriodsService, ContractPeriodsService>();
        services.AddScoped<IEmployeesService, EmployeesService>();
        services.AddScoped<IKPAService, KPAService>();
        services.AddScoped<IKPIService, KPIService>();
        services.AddScoped<IRatingPeriodsServices, RatingPeriodsServices>();
        services.AddScoped<IScoringPeriodsService, ScoringPeriodsService>();
        services.AddScoped<IKPALinksService, KPALinksService>();
        services.AddScoped<IContractResultsService, ContractResultsService>();
        services.AddScoped<ICommentsAPIService, CommentsAPIService>();
    }

}
