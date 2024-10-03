using System.Reflection;
using Customer.Application.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Application
    {
    public static class ApplicationServiceRegistration
        {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(conf =>
            {
                conf.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
        }
    }