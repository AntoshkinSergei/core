using Autofac;
using DecisionMapper.Data;

namespace DecisionMapper.IoC
{
    public class DataModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterType<DbContext>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(typeof(DbContext).Assembly)
                .Where(x => x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
        }
    }
}
