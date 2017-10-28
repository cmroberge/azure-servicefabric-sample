using Microsoft.ServiceFabric.Services.Communication.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using Quartz;
using TimeTriggerService;
using Quartz.Impl;

namespace SimpleTimeTriggerService
{
    public class SimpleTimeTriggerListener : ICommunicationListener
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
            var tasks =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(
                        t =>
                            typeof(IJob).IsAssignableFrom(t) &&
                            Attribute.IsDefined(t, typeof(JobInfoAttribute)));

            foreach (var task in tasks)
            {
                var schedulerFactory = new StdSchedulerFactory();
                scheduler = schedulerFactory.GetScheduler();
                var jobInfo = Attribute.GetCustomAttribute(task, typeof(JobInfoAttribute)) as JobInfoAttribute;
                var jobName = jobInfo == null ? task.Name : jobInfo.Name;
                var job = new JobDetailImpl(jobName, null, task);

                var trigger =
                    TriggerBuilder.Create()
                        .WithIdentity($"{jobName}Trigger", null)
                        .WithCronSchedule(jobInfo.CronExpression)
                        .ForJob(job)
                        .Build();
                scheduler.ScheduleJob(job, trigger);                
            }

            scheduler.Start();

            return Task.FromResult("Sample Simple Job scheduler");
        }
    }
}
