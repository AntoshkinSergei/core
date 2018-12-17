using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using DecisionMapper.Infrastructure;
using DecisionMapper.IoC;
using Microsoft.Extensions.Configuration;

namespace DecisionMapper.WebApi
{
    public static class ContainerConfig
    {
        public static void Configure(ContainerBuilder builder, IConfiguration config)
        {
            // Register database connection settings
            var connectionString = config.GetSection("MongoConnection:ConnectionString").Value;
            var database = config.GetSection("MongoConnection:Database").Value;

            builder
                .RegisterType<DatabaseSettings>()
                .WithParameters(new List<NamedParameter>()
                {
                    new NamedParameter("connectionString", connectionString),
                    new NamedParameter("database", database)
                })
                .AsSelf()
                .InstancePerLifetimeScope();

            // Register internal project's modules
            builder.RegisterModule<DataModule>();
            builder.RegisterModule<ServicesModule>();

            // Register WebApi project types
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("Mapper"))
                .AsSelf()
                .AsImplementedInterfaces();

            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("Helper"))
                .AsSelf()
                .AsImplementedInterfaces();
        }
    }
}
