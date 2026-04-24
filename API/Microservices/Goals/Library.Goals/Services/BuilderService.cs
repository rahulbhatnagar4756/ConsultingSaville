using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Goals.Services.Contracts;
using Library.Goals.Services.Comments;

namespace Library.Goals.Services;

public static class BuilderService
{
    public static void AddGoalsServices(this IServiceCollection services)
    {
        services.AddScoped<ITemplatesService, TemplatesService>();
        services.AddScoped<IContractService, ContractService>();
        services.AddScoped<IKPAService, KPAService>();
        services.AddScoped<IKPIService, KPIService>();
        services.AddScoped<ITolerancesService, TolerancesService>();
        services.AddScoped<IPillarsService, PillarsService>();
        services.AddScoped<IStatusService, StatusService>();
        services.AddScoped<IEnterpriseStructuresService, EnterpriseStructuresService>();
        services.AddScoped<IContractsDataService, ContractsDataService>();
        services.AddScoped<IContractPeriodsService, ContractPeriodsService>();
        services.AddScoped<IEmployeesService, EmployeesService>(); 
        services.AddScoped<IRatingPeriodsService, RatingPeriodsService>();
        services.AddScoped<IScoringPeriodsService, ScoringPeriodsService>();
        services.AddScoped<IKPALinksService, KPALinksService>();
        services.AddScoped<IContractResultsService, ContractResultsService>();
        services.AddScoped<ICommentsService, CommentsService>();
        services.AddScoped<ICommentsAttachmentService, CommentsAttachmentService>();
    }
}
