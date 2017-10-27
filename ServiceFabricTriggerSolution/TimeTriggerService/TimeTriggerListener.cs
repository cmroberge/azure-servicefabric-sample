using Microsoft.ServiceFabric.Services.Communication.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Autofac;
using Quartz;
using Autofac.Extras.Quartz;
using System.Reflection;

namespace TimeTriggerService
{
    internal class TimeTriggerListener : ICommunicationListener
    {
        private IScheduler scheduler;

        public void Abort()
        {
            scheduler.Shutdown(false);
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            scheduler.Shutdown(true);
            return Task.FromResult(true);
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new QuartzAutofacFactoryModule());
            builder.RegisterModule(new QuartzAutofacJobsModule(Assembly.GetExecutingAssembly()));
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(x => typeof(IJob).IsAssignableFrom(x)).As<IJob>();
            builder.RegisterType<Logger>().As<ILogger>();
            builder.RegisterType<JobScheduler>().AsSelf();
            IContainer container = builder.Build();
            ConfigureScheduler(container);

            return Task.FromResult("Sample Job scheduler");
        }
        
        private void ConfigureScheduler(IContainer container)
        {
            IEnumerable<IJob> jobList = container.Resolve<IEnumerable<IJob>>();
            var jobScheduler = container.Resolve<JobScheduler>();
            this.scheduler = jobScheduler.Start();
        }
    }
}
