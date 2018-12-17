using Autofac;
using DecisionMapper.Services;

namespace DecisionMapper.IoC
{
    public class ServicesModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<CarsService>().AsImplementedInterfaces();
        }
    }
}
