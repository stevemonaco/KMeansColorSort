using Autofac;
using KMeansColorSort.Services;
using KMeansColorSort.ViewModels;

namespace KMeansColorSort
{
    class AppBootstrapper : AutofacBootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(ContainerBuilder builder)
        {
            builder.RegisterType<ColorGeneratorService>().As<IColorGeneratorService>();
        }
    }
}
