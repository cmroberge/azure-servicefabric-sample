# azure-servicefabric-sample  

### Introduction  
This repository is aimed to hold Azure Service Fabric related samples.  
Azure Service Fabric Servies can be configured to exhibit trigger based bahvior similar to [WebJobs](https://docs.microsoft.com/en-us/azure/app-service/web-sites-create-web-jobs) and [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview). Have added a code samples to exhibit such behavior (Detailed below).  
### Solutions
- [Time triggered / Scheduler Service in Azure Service Fabric](#Time-triggered-/-Scheduler-Service-in-Azure-Service-Fabric-(ServiceFabricTriggerSolution))  
#### Time triggered / Scheduler Service in Azure Service Fabric (ServiceFabricTriggerSolution)
To create a time triggered (scheduler) Service in Azure Service Fabric, I can think of following two quick options among the possible ways.
1. Create a time triggered WebJob and add/deploy it as a [guest executable](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-deploy-existing-app) in Azure Service Fabric. 
2. Create Azure Service Fabric Stateless Service and implement the listener to handle jobs.  

Here I'm taking the second approach which involves the respective listener to be created. Rather than writing new Scheduling framework, I'm using the [Quartz.Net](https://www.quartz-scheduler.net/) framework. I'm using the CRON expression behavior to have the behavior inline with the time trigger behavior of [WebJobs](https://docs.microsoft.com/en-us/azure/app-service/web-sites-create-web-jobs) and [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview).  
- Solution: [ServiceFabricTriggerSolution](https://github.com/titodotnet/azure-servicefabric-sample/tree/master/ServiceFabricTriggerSolution)
- Projects:
   1. [SimpleTimeTriggerService](https://github.com/titodotnet/azure-servicefabric-sample/tree/master/ServiceFabricTriggerSolution/SimpleTimeTriggerService) 
   2. [TimeTriggerService](https://github.com/titodotnet/azure-servicefabric-sample/tree/master/ServiceFabricTriggerSolution/TimeTriggerService)  

##### SimpleTimeTriggerService
- Uses [Quartz.Net](https://www.quartz-scheduler.net/) framework.
- Without Dependency Injection.
- Listerner gets all the `IJob` types from the current assembly with `JobInfoAttribute` decorated.
- `JobInfoAttribute` holds the name and CRON expression of the job.  

##### TimeTriggerService
- Uses [Quartz.Net](https://www.quartz-scheduler.net/) framework.
- With Dependency Injection using [Autofac (Autofac integration package for Quartz.Net)](https://github.com/alphacloud/Autofac.Extras.Quartz).
- Listerner gets all the `IJob` types from the current assembly with `JobInfoAttribute` decorated.
- `JobInfoAttribute` holds the name and CRON expression of the job.
