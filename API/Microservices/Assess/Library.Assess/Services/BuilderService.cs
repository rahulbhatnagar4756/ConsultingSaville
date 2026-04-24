using Library.Assess.Services.Jobs;
using Library.Assess.Services.PerformanceReviewReports;
using Library.Assess.Services.PersonalityPDF;
using Library.Assess.Services.PersonalityWheel;
using Library.Assess.Services.Projects;
using Library.Assess.Services.ReportPDF;
using Library.Assess.Services.SelectionStrategyReport;
using Library.Assess.Services.TalentFitReport;
using Library.Assess.Services.TalentMatchReportPDF;
using Library.Assess.Services.TalentMatchSelectionReportPDF;

using Microsoft.Extensions.DependencyInjection;

namespace Library.Assess.Services
{
    public static class BuilderService
    {
        public static void AddAssessServices(this IServiceCollection services)
        {
            services.AddScoped<IJobsService, JobsService>();
            services.AddScoped<IProjectsUsersService, ProjectsUsersService>();
            services.AddScoped<IAssessmentUsersService, AssessmentUsersService>();
            services.AddScoped<IProjectsService, ProjectsService>();
            services.AddScoped<IPersonalityWheelRepository, PersonalityWheelRepository>();
            services.AddScoped<IPdfReportService, PdfReportService>();
            services.AddScoped<ITalentMatchReportPDFService, TalentMatchReportPDFService>();
            services.AddScoped<ITalentMatchSelectionReportPDFService, TalentMatchSelectionReportPDFService>();
            services.AddScoped<ISelectionStrategyReportService, SelectionStrategyReportService>();
            services.AddScoped<IPersonalityPdfService, PersonalityPdfService>();
            services.AddScoped<ITalentFitReport_DevelopmentService, TalentFitReport_DevelopmentService>();
            services.AddScoped<ITalentFitReport_SelectionService, TalentFitReport_SelectionService>();
            services.AddScoped<ITalentFitReport_InterviewService, TalentFitReport_InterviewService>(); 
            services.AddScoped<IPerformanceReviewService, PerformanceReviewService>();
        }
    }
}
