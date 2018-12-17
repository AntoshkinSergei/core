using Autofac;
using DecisionMapper.Data;
using DecisionMapper.Entities;
using DecisionMapper.Entities.Infrastructure;
using DecisionMapper.Services;

namespace DecisionMapper.Testing.Infrastructure
{
    public static class ContainerConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            ConfigureDataModules(builder);
            ConfigureServices(builder);
        }

        private static void ConfigureDataModules(ContainerBuilder builder)
        {
            builder
                .RegisterType<DbContextMocked>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<MongoCollectionMocked<Car>>().AsImplementedInterfaces();
            builder.RegisterType<MongoCollectionMocked<Sequence>>().AsImplementedInterfaces();

            builder.RegisterType<AsyncCursorMocked<Car>>().AsImplementedInterfaces();
            builder.RegisterType<AsyncCursorMocked<Sequence>>().AsImplementedInterfaces();

            builder
                .RegisterAssemblyTypes(typeof(DbContext).Assembly)
                .Where(x => x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
        }

        private static void ConfigureServices(ContainerBuilder builder)
        {
            builder.RegisterType<CarsService>().AsImplementedInterfaces();
        }
    }
}
