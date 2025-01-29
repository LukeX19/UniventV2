﻿using Univent.App.Interfaces;
using Univent.Infrastructure;
using Univent.Infrastructure.Repositories;

namespace Univent.Api.Extensions
{
    public static class ServiceConfigurator
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUniversityRepository, UniversityRepository>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IEventTypeRepository, EventTypeRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
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
