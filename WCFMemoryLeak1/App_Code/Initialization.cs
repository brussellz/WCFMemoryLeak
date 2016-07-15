using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Integration.Wcf;
using Microsoft.Practices.ServiceLocation;
using WCFMemoryLeak1.Service;
using WCFMemoryLeak1.Service.Interface;

namespace WCFMemoryLeak1.App_Code
{
    public static class Initialization
    {
        public static void AppInitialize()
        {
            var builder = new ContainerBuilder();

            // Register Types
            builder.RegisterType<DataService>().AsSelf();
            builder.RegisterType<DataAccessService>().As<IDataAccessService>();

            var container = builder.Build();

            // Setup Service Locator
            var serviceLocator = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);

            // Setup Autofac.Integration.WCF
            AutofacHostFactory.Container = container;
        }
    }
}