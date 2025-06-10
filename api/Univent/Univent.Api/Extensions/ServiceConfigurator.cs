using Univent.App.Interfaces;
using Univent.Infrastructure;
using Univent.Infrastructure.Repositories;
using Univent.Infrastructure.Services;

namespace Univent.Api.Extensions
{
    public static class ServiceConfigurator
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUniversityRepository, UniversityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEventTypeRepository, EventTypeRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventParticipantRepository, EventParticipantRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IFileService, FileService>();
            services.AddHttpClient<IWeatherService, WeatherService>();
            services.AddScoped<IAiAssistantService, GitHubInferenceAiAssistantService>();
        }

        public static void AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IUniversityRepository).Assembly));
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
