using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using log4net.Config;
using Quartz;
using Autofac;
using Autofac.Extras.Quartz;
using System.Collections.Specialized;
using System.Configuration;
using System.Runtime;
using System.Reflection;

namespace AAAServiceByTopShelf
{
    public class Program
    {
        static void Main(string[] args)
        {

            XmlConfigurator.Configure();
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Service>()
                            .AsSelf()
                            .InstancePerLifetimeScope();


            builder.RegisterModule(new QuartzAutofacFactoryModule
            {
                ConfigurationProvider = context =>
                    (NameValueCollection)ConfigurationManager.GetSection("quartz")
            });

            builder.RegisterModule(new QuartzAutofacJobsModule(Assembly.GetExecutingAssembly()));

            IContainer container = builder.Build();
            HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.SetServiceName("AAService");
                hostConfigurator.SetDisplayName("AA Service Test");
                hostConfigurator.SetDescription("Does custom logic.");

                hostConfigurator.RunAsLocalSystem();
                hostConfigurator.UseLog4Net();

                hostConfigurator.Service<Service>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(hostSeting =>container.Resolve<Service>());

                    serviceConfigurator.WhenStarted(service => service.OnStart());
                    serviceConfigurator.WhenStopped(service => service.OnStop());

                });

            });
        }
    }
}
